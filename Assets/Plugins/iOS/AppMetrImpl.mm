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

- (void)setKey:(NSString*)key Value:(NSString*)value
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

	void _setKeyValue(const char* key, const char* value)
	{
		[[AppMetrImpl sharedAppMetrImpl] setKey:createNSString(key) Value:createNSString(value)];
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
		[AppMetr trackSessionWithProperties:[[AppMetrImpl sharedAppMetrImpl] keyValueDict]];
		[[AppMetrImpl sharedAppMetrImpl] resetDict];
	}
	
	void _trackLevel(int level)
	{
		[AppMetr trackLevel:level];
	}
	
	void _trackLevelWithProperties(int level)
	{
		[AppMetr trackLevel:level properties:[[AppMetrImpl sharedAppMetrImpl] keyValueDict]];
		[[AppMetrImpl sharedAppMetrImpl] resetDict];
	}
	
	void _trackEvent(const char* event)
	{
		[AppMetr trackEvent:createNSString(event)];
	}
	
	void _trackEventWithProperties(const char* event)
	{
		[AppMetr trackEvent:createNSString(event) properties:[[AppMetrImpl sharedAppMetrImpl] keyValueDict]];
		[[AppMetrImpl sharedAppMetrImpl] resetDict];
	}
	
	void _trackPayment()
	{
		[AppMetr trackPayment:paymentWithPaymentProcessor([[AppMetrImpl sharedAppMetrImpl] keyValueDict])];
		[[AppMetrImpl sharedAppMetrImpl] resetDict];
	}
	
	void _trackPaymentWithProperties()
	{
		[AppMetr trackPayment:paymentWithPaymentProcessor([[AppMetrImpl sharedAppMetrImpl] keyValueDict]) properties:[[AppMetrImpl sharedAppMetrImpl] keyValueDictOptional]];
		[[AppMetrImpl sharedAppMetrImpl] resetDict];
	}
	
	void _trackOptions(const char* commandId)
	{
		[AppMetr trackOptions:[[AppMetrImpl sharedAppMetrImpl] keyValueDict] forCommand:createNSString(commandId)];
		[[AppMetrImpl sharedAppMetrImpl] resetDict];
	}
	
	void _trackOptionsWithErrorCode(const char* commandId, const char* code, const char* message)
	{
		[AppMetr trackOptions:[[AppMetrImpl sharedAppMetrImpl] keyValueDict] forCommand:createNSString(commandId) errorCode:createNSString(code) errorMessage:createNSString(message)];
		[[AppMetrImpl sharedAppMetrImpl] resetDict];
	}
	
}
