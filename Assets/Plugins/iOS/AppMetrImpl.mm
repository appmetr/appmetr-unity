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

- (void)setKeyOptional:(NSString*)key Value:(NSString*)value
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

	void _setKeyValueNumber(const char* key, double value)
	{
		[[AppMetrImpl sharedAppMetrImpl] setKeyNumber:createNSString(key) Value:[NSNumber numberWithDouble: value]];
	}
	
	void _setKeyValueOptional(const char* key, const char* value)
	{
		[[AppMetrImpl sharedAppMetrImpl] setKeyOptional:createNSString(key) Value:createNSString(value)];
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
	
	void _trackEvent(const char* event)
	{
		[AppMetr trackEvent:createNSString(event)];
	}
	
	void _trackEventWithProperties(const char* event)
	{
		NSMutableDictionary* dictCopy = [[[AppMetrImpl sharedAppMetrImpl] keyValueDict] copy];
		[AppMetr trackEvent:createNSString(event) properties:dictCopy];
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
	
}
