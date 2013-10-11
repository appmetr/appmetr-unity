#import "AppMetrImpl.h"

static AppMetrImpl *_sharedInstance = nil; // To make AppMetrImpl Singleton

@implementation AppMetrImpl

@synthesize keyValueDict = keyValueDict_;
@synthesize keyValueDictOptional = keyValueDictOptional_;
@synthesize appMetrListener = appMetrListener_;

+ (void)initialize
{
	if (self == [AppMetrImpl class])
	{
		_sharedInstance = [[self alloc] init];
	}
}

+ (AppMetrImpl*)sharedAppMetrImpl
{
	return _sharedInstance;
}

- (id)init
{
	self = [super init];
	
	[appMetrListener_ release];
	appMetrListener_ = [[AppMetrListener alloc] init];
	
	return self;
}

- (void)dealloc
{
	[appMetrListener_ release];
	[super dealloc];
}

- (void)setKeyString:(NSString*)key Value:(NSString*)value
{
	if (!keyValueDict_)
		keyValueDict_ = [[NSMutableDictionary alloc] init];
	[keyValueDict_ setObject:value forKey:key];
}

- (void)setKeyNumber:(NSString*)key Value:(NSNumber*)value
{
	if (!keyValueDict_)
		keyValueDict_ = [[NSMutableDictionary alloc] init];
	[keyValueDict_ setObject:value forKey:key];
}

- (void)setKeyStringOptional:(NSString*)key Value:(NSString*)value
{
	if (!keyValueDictOptional_)
		keyValueDictOptional_ = [[NSMutableDictionary alloc] init];
	[keyValueDictOptional_ setObject:value forKey:key];
}

- (void)setKeyNumberOptional:(NSString*)key Value:(NSNumber*)value
{
	if (!keyValueDictOptional_)
		keyValueDictOptional_ = [[NSMutableDictionary alloc] init];
	[keyValueDictOptional_ setObject:value forKey:key];
}

- (void)resetDict
{
	if (keyValueDict_)
		[keyValueDict_ removeAllObjects];
	if (keyValueDictOptional_)
		[keyValueDictOptional_ removeAllObjects];
}

@end


NSString* createNSString(const char* string)
{
	if (string)
		return [NSString stringWithUTF8String: string];
	else
		return [NSString stringWithUTF8String: ""];
}

NSDictionary* paymentWithPaymentProcessor(NSDictionary *dict)
{
	if (![dict objectForKey:@"processor"])
	{
		dict = [[dict mutableCopy] autorelease];
		[dict setValue:@"appstore" forKey:@"processor"];
	}
	return dict;
}

	
extern "C" {

	void _setKeyValueString(const char* key, const char* value)
	{
		[[AppMetrImpl sharedAppMetrImpl] setKeyString:createNSString(key) Value:createNSString(value)];
	}

	void _setKeyValueFloat(const char* key, float value)
	{
		[[AppMetrImpl sharedAppMetrImpl] setKeyNumber:createNSString(key) Value:[NSNumber numberWithFloat: value]];
	}

	void _setKeyValueInt(const char* key, int value)
	{
		[[AppMetrImpl sharedAppMetrImpl] setKeyNumber:createNSString(key) Value:[NSNumber numberWithInteger: value]];
	}
	
	void _setKeyValueStringOptional(const char* key, const char* value)
	{
		[[AppMetrImpl sharedAppMetrImpl] setKeyStringOptional:createNSString(key) Value:createNSString(value)];
	}

	void _setKeyValueFloatOptional(const char* key, float value)
	{
		[[AppMetrImpl sharedAppMetrImpl] setKeyNumberOptional:createNSString(key) Value:[NSNumber numberWithFloat: value]];
	}

	void _setKeyValueIntOptional(const char* key, int value)
	{
		[[AppMetrImpl sharedAppMetrImpl] setKeyNumberOptional:createNSString(key) Value:[NSNumber numberWithInteger: value]];
	}

	void _setupWithToken(const char* token)
	{
		[AppMetr setupWithToken:createNSString(token) delegate:[[AppMetrImpl sharedAppMetrImpl] appMetrListener]];
	}
	
	void _trackSession()
	{
		[AppMetr trackSession];
	}
	
	void _trackSessionWithProperties()
	{
		NSMutableDictionary* dictCopy = [[[AppMetrImpl sharedAppMetrImpl] keyValueDict] copy];
		[AppMetr trackSessionWithProperties:dictCopy];
		[[AppMetrImpl sharedAppMetrImpl] resetDict];
	}
	
	void _trackLevel(int level)
	{
		[AppMetr trackLevel:level];
	}
	
	void _trackLevelWithProperties(int level)
	{
		NSMutableDictionary* dictCopy = [[[AppMetrImpl sharedAppMetrImpl] keyValueDict] copy];
		[AppMetr trackLevel:level properties:dictCopy];
		[[AppMetrImpl sharedAppMetrImpl] resetDict];
	}
	
	void _trackEvent(const char* eventName)
	{
		[AppMetr trackEvent:createNSString(eventName)];
	}
	
	void _trackEventWithProperties(const char* eventName)
	{
		NSMutableDictionary* dictCopy = [[[AppMetrImpl sharedAppMetrImpl] keyValueDict] copy];
		[AppMetr trackEvent:createNSString(eventName) properties:dictCopy];
		[[AppMetrImpl sharedAppMetrImpl] resetDict];
	}
	
	void _trackPayment()
	{
		NSMutableDictionary* dictCopy = [[[AppMetrImpl sharedAppMetrImpl] keyValueDict] copy];
		[AppMetr trackPayment:paymentWithPaymentProcessor(dictCopy)];
		[[AppMetrImpl sharedAppMetrImpl] resetDict];
	}
	
	void _trackPaymentWithProperties()
	{
		NSMutableDictionary* dictCopy = [[[AppMetrImpl sharedAppMetrImpl] keyValueDict] copy];
		NSMutableDictionary* dictOptCopy = [[[AppMetrImpl sharedAppMetrImpl] keyValueDictOptional] copy];
		[AppMetr trackPayment:paymentWithPaymentProcessor(dictCopy) properties:dictOptCopy];
		[[AppMetrImpl sharedAppMetrImpl] resetDict];
	}
	
	void _attachProperties()
	{
		NSMutableDictionary* dictCopy = [[[AppMetrImpl sharedAppMetrImpl] keyValueDict] copy];
		[AppMetr attachProperties:dictCopy];
		[[AppMetrImpl sharedAppMetrImpl] resetDict];
	}
	
	void _trackOptions(const char* commandId)
	{
		NSMutableDictionary* dictCopy = [[[AppMetrImpl sharedAppMetrImpl] keyValueDict] copy];
		[AppMetr trackOptions:dictCopy forCommand:createNSString(commandId)];
		[[AppMetrImpl sharedAppMetrImpl] resetDict];
	}
	
	void _trackOptionsWithErrorCode(const char* commandId, const char* code, const char* message)
	{
		NSMutableDictionary* dictCopy = [[[AppMetrImpl sharedAppMetrImpl] keyValueDict] copy];
		[AppMetr trackOptions:dictCopy forCommand:createNSString(commandId) errorCode:createNSString(code) errorMessage:createNSString(message)];
		[[AppMetrImpl sharedAppMetrImpl] resetDict];
	}
	
	void _trackExperimentStart(const char* experiment, const char* groupId)
	{
		[AppMetr trackExperimentStart:createNSString(experiment) group:createNSString(groupId)];
	}
	
	void _trackExperimentEnd(const char* experiment)
	{
		[AppMetr trackExperimentEnd:createNSString(experiment)];
	}
	
	void _identify(const char* userId)
	{
		[AppMetr identify:createNSString(userId)];
	}
	
	void _flush()
	{
		[AppMetr flush];
	}
}
