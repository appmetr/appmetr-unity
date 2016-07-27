#import "AppMetrImpl.h"

static AppMetrListener * appMetrListener_ = nil;

@implementation AppMetrImpl

+ (AppMetrListener*)appMetrListener
{
	return appMetrListener_;
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

@end


NSString* charToNSString(const char* string)
{
	if (string)
		return [NSString stringWithUTF8String: string];
	else
		return [NSString stringWithUTF8String: ""];
}

char* nsStringToChar(NSString* string)
{
	const char* charStr = [string UTF8String];

    if (charStr == NULL)
        return NULL;
    
    char* res = (char*)malloc(strlen(charStr) + 1);
    strcpy(res, charStr);
    
    return res;
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

	void _setupWithToken(const char* token)
	{
		[AppMetr setupWithToken:charToNSString(token)];
	}
	
	void _trackSession()
	{
		[AppMetr trackSession];
	}
	
	void _trackSessionWithProperties(const char* properties)
	{
		NSDictionary* dict = [AppMetr stringToDictionary:charToNSString(properties)];
		[AppMetr trackSessionWithProperties:dict];
	}
	
	void _trackLevel(int level)
	{
		[AppMetr trackLevel:level];
	}
	
	void _trackLevelWithProperties(int level, const char* properties)
	{
		NSDictionary* dict = [AppMetr stringToDictionary:charToNSString(properties)];
		[AppMetr trackLevel:level properties:dict];
	}
	
	void _trackEvent(const char* eventName)
	{
		[AppMetr trackEvent:charToNSString(eventName)];
	}
	
	void _trackEventWithProperties(const char* eventName, const char* properties)
	{
		NSDictionary* dict = [AppMetr stringToDictionary:charToNSString(properties)];
		[AppMetr trackEvent:charToNSString(eventName) properties:dict];
	}
	
	void _trackPayment(const char* payment)
	{
		NSDictionary* dict = [AppMetr stringToDictionary:charToNSString(payment)];
		[AppMetr trackPayment:paymentWithPaymentProcessor(dict)];
	}

	void _trackAdsEvent(const char* event)
	{
		[AppMetr trackAdsEvent:charToNSString(event)];
	}
	
	void _trackPaymentWithProperties(const char* payment, const char* properties)
	{
		NSDictionary* dictPayment = [AppMetr stringToDictionary:charToNSString(payment)];
		NSDictionary* dictProperties = [AppMetr stringToDictionary:charToNSString(properties)];
		[AppMetr trackPayment:paymentWithPaymentProcessor(dictPayment) properties:dictProperties];
	}
	
	void _attachPropertiesNull()
	{
		[AppMetr attachProperties];
	}
	
	void _attachProperties(const char* properties)
	{
		NSDictionary* dict = [AppMetr stringToDictionary:charToNSString(properties)];
		[AppMetr attachProperties:dict];
	}
	
	void _trackOptions(const char* commandId, const char* options)
	{
		NSArray* dict = [AppMetr stringToArray:charToNSString(options)];
		[AppMetr trackOptions:dict forCommand:charToNSString(commandId)];
	}
	
	void _trackOptionsWithErrorCode(const char* commandId, const char* options, const char* code, const char* message)
	{
		NSArray* dict = [AppMetr stringToArray:charToNSString(options)];
		[AppMetr trackOptions:dict forCommand:charToNSString(commandId) errorCode:charToNSString(code) errorMessage:charToNSString(message)];
	}
	
	void _trackExperimentStart(const char* experiment, const char* groupId)
	{
		[AppMetr trackExperimentStart:charToNSString(experiment) group:charToNSString(groupId)];
	}
	
	void _trackExperimentEnd(const char* experiment)
	{
		[AppMetr trackExperimentEnd:charToNSString(experiment)];
	}

	bool _verifyPayment(const char* productId, const char* transactionId, const char* receipt, const char* privateKey) 
	{
		return [AppMetr verifyPaymentWithProductId:charToNSString(productId) 
											transactionId:charToNSString(transactionId) 
												  receipt:charToNSString(receipt) 
											   privateKey:charToNSString(privateKey)];
	}

	void _trackState(const char* state)
	{
		NSDictionary* dict = [AppMetr stringToDictionary:charToNSString(state)];
		[AppMetr trackState:dict];
	}
	
	void _identify(const char* userId)
	{
		[AppMetr identify:charToNSString(userId)];
	}
	
	void _flush()
	{
		[AppMetr flush];
	}
	
	char* _instanceIdentifier()
	{
		return nsStringToChar([AppMetr instanceIdentifier]);
	}
}
