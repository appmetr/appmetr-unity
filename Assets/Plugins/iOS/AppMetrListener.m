#import "AppMetrListener.h"
#import "AppMetr.h"

@implementation AppMetrListener

@synthesize unityMessageRecipient;

-(void)dealloc
{
    unityMessageRecipient = nil;
	[super dealloc];
}

#pragma mark - AppMetrDelegate

-(void)executeCommand:(NSDictionary *)command
{
    if(unityMessageRecipient == nil || unityMessageRecipient.length == 0)
        return;
	NSError *serializeError = nil;
	NSData *data = [NSJSONSerialization dataWithJSONObject:command options:0 error:&serializeError];
	if (serializeError)
	{
		NSLog(@"JSON srializer error: %@", serializeError.description);
		[NSException raise:NSGenericException
					format:@"%@", serializeError.description];
	}
	else
	{
		NSString *serializedData = [[[NSString alloc] initWithData:data encoding:NSUTF8StringEncoding] autorelease];
		if (serializedData)
		{
			UnitySendMessage([unityMessageRecipient cStringUsingEncoding:NSUTF8StringEncoding], "OnAppMetrCommand", [serializedData UTF8String]);
		}
	}
}

@end
