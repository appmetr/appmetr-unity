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
	
	void _trackSessionWithProperties(const char* properties)
	{
		NSDictionary* dict = [AppMetr stringToDictionary:createNSString(properties)];
		[AppMetr trackSessionWithProperties:dict];
	}
	
	void _trackLevel(int level)
	{
		[AppMetr trackLevel:level];
	}
	
	void _trackLevelWithProperties(int level, const char* properties)
	{
		NSDictionary* dict = [AppMetr stringToDictionary:createNSString(properties)];
		[AppMetr trackLevel:level properties:dict];
	}
	
	void _trackEvent(const char* eventName)
	{
		[AppMetr trackEvent:createNSString(eventName)];
	}
	
	void _trackEventWithProperties(const char* eventName, const char* properties)
	{
		NSDictionary* dict = [AppMetr stringToDictionary:createNSString(properties)];
		[AppMetr trackEvent:createNSString(eventName) properties:dict];
	}
	
	void _trackPayment(const char* payment)
	{
		NSDictionary* dict = [AppMetr stringToDictionary:createNSString(payment)];
		[AppMetr trackPayment:paymentWithPaymentProcessor(dict)];
	}
	
	void _trackPaymentWithProperties(const char* payment, const char* properties)
	{
		NSDictionary* dictPayment = [AppMetr stringToDictionary:createNSString(payment)];
		NSDictionary* dictProperties = [AppMetr stringToDictionary:createNSString(properties)];
		[AppMetr trackPayment:paymentWithPaymentProcessor(dictPayment) properties:dictProperties];
	}
	
	void _attachPropertiesNull()
	{
		[AppMetr attachProperties];
	}
	
	void _attachProperties(const char* properties)
	{
		NSDictionary* dict = [AppMetr stringToDictionary:createNSString(properties)];
		[AppMetr attachProperties:dict];
	}
	
	void _trackOptions(const char* options, const char* commandId)
	{
		NSDictionary* dict = [AppMetr stringToDictionary:createNSString(options)];
		[AppMetr trackOptions:dict forCommand:createNSString(commandId)];
	}
	
	void _trackOptionsWithErrorCode(const char* options, const char* commandId, const char* code, const char* message)
	{
		NSDictionary* dict = [AppMetr stringToDictionary:createNSString(options)];
		[AppMetr trackOptions:dict forCommand:createNSString(commandId) errorCode:createNSString(code) errorMessage:createNSString(message)];
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
