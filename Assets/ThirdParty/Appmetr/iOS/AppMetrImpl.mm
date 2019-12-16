#import "AppMetr.h"

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
		dict = [dict mutableCopy];
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
    
    void _flushLocal()
    {
        [AppMetr flushLocal];
    }
	
	char* _instanceIdentifier()
	{
		return nsStringToChar([AppMetr instanceIdentifier]);
	}

	char* _deviceKey()
	{
		return nsStringToChar([AppMetr deviceKey]);
	}

    void _attachEntityAttributes(const char* entityName, const char* entityValue, const char* properties)
    {
        NSDictionary* props = [AppMetr stringToDictionary:charToNSString(properties)];
        [AppMetr attachEntityAttributesForName:charToNSString(entityName) value:charToNSString(entityValue) withProperies:props];
    }
}
