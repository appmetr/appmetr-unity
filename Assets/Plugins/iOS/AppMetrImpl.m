#include "AppMetr.h"
#include "AppMetrImpl.h"
#import "CJSONDeserializer.h"

NSDictionary *deserializeJson(const char* properties)
{
	@try
	{
		NSError *jsonError = nil;
		CJSONDeserializer *deserializer = [CJSONDeserializer deserializer];
		NSString *jsonString = [NSString stringWithUTF8String:properties];
		NSData *jsonData = [jsonString dataUsingEncoding:NSUTF8StringEncoding];
		NSDictionary *result = [deserializer deserializeAsDictionary:jsonData
															   error:&jsonError];
		if(!jsonError)
		{
			assert(result);
			return result;
		}

		NSLog(@"An error occured while partsing JSON: %@", jsonError);
	}
	@catch(NSException *exception)
	{
		NSLog(@"An exception throwed while partsing JSON: %@",
				exception.description);
	}

	return nil;
}

NSString* createNSString(const char* string)
{
	if (string)
		return [NSString stringWithUTF8String: string];
	else
		return [NSString stringWithUTF8String: ""];
}

NSDictionary* paymentWithPaymentProcessor(NSDictionary *json)
{
	if (![json objectForKey:@"processor"])
	{
		json = [[json mutableCopy] autorelease];
		[json setValue:kPaymentProcessor forKey:@"processor"];
	}
	
	return json;
}
	
extern "C" {

	void _setupWithToken(const char* token)
	{
		[AppMetr setupWithToken:createNSString(token)];
	}
	
	void _setupWithUserID(const char* userID)
	{
		[AppMetr setupWithUserID:createNSString(userID)];
	}

	void _attachProperties(const char* properties)
	{
		NSDictionary *json = deserializeJson(properties);
		if (json)
		{
			[AppMetr attachProperties:json];
		}
	}
	
	void _trackSession()
	{
		[AppMetr trackSession];
	}
	
	void _trackSessionWithProperties(const char* properties)
	{
		NSDictionary *json = deserializeJson(properties);
		if (json)
		{
			[AppMetr trackSessionWithProperties:json];
		}
	}
	
	void _trackLevel(int level)
	{
		[AppMetr trackLevel:level];
	}
	
	void _trackLevel(int level, const char* properties)
	{
		NSDictionary *json = deserializeJson(properties);
		if (json)
		{
			[AppMetr trackLevel:level properties:json];
		}
	}
	
	void _trackEvent(const char* event)
	{
		[AppMetr trackEvent:createNSString(event)];
	}
	
	void _trackEvent(const char* event, const char* properties)
	{
		NSDictionary *json = deserializeJson(properties);
		if (json)
		{
			[AppMetr trackEvent:createNSString(event) properties:json];
		}
	}
	
	void _trackPayment(const char* payment)
	{
		NSDictionary *json = deserializeJson(payment);
		if (json)
		{
			json = paymentWithPaymentProcessor(json);
			[AppMetr trackPayment:json];
		}
	}
	
	void _trackPayment(const char* payment, const char* properties)
	{
		NSDictionary *jsonPayment = deserializeJson(payment);
		NSDictionary *jsonProperties = deserializeJson(properties);
		if (jsonPayment && jsonProperties)
		{
			jsonPayment = paymentWithPaymentProcessor(jsonPayment);
			[AppMetr trackPayment:jsonPayment properties:jsonProperties];
		}
	}
	
	void _trackGameState(const char* state, const char* properties)
	{
		NSDictionary *json = deserializeJson(properties);
		if (json)
		{
			[AppMetr trackPayment:createNSString(state) properties:json];
		}
	}
	
	void _trackOptions(const char* options, const char* commandId)
	{
		NSDictionary *json = deserializeJson(options);
		if (json)
		{
			[AppMetr trackOptions:json forCommand:createNSString(commandId)];
		}
	}
	
	void _trackOptions(const char* options, const char* commandId, const char* code, const char* message)
	{
		NSDictionary *json = deserializeJson(options);
		if (json)
		{
			[AppMetr trackOptions:json forCommand:createNSString(commandId) errorCode:createNSString(code) errorMessage:createNSString(message)];
		}
	}
	
	void _trackExperimentStart(const char* experiment, const char* group)
	{
		[AppMetr trackExperimentStart:createNSString(experiment) group:createNSString(group)];
	}
	
	void _trackExperimentEnd(const char* experiment)
	{
		[AppMetr trackExperimentEnd:createNSString(experiment)];
	}
	
	void _pullCommands()
	{
		[AppMetr pullCommands];
	}

	void _flush()
	{
		[AppMetr flush];
	}
	
	void _setDebugLoggingEnabled(bool debugLoggingEnabled)
	{
		[AppMetr setDebugLoggingEnabled:debugLoggingEnabled];
	}
	
}
