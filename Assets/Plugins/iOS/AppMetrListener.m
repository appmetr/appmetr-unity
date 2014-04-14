#import "AppMetrListener.h"
#import "AppMetr.h"
#import "AppMetrImpl.h"
#import "CJSONSerializer.h"

@implementation AppMetrListener
{
}

-(id)init
{
	self = [super init];
	return self;
}

-(void)dealloc
{
	[super dealloc];
}

#pragma mark - AppMetrDelegate

-(void)executeCommand:(NSDictionary *)command
{
	NSError *serializeError = nil;
	NSData *data = [[AMCJSONSerializer serializer] serializeDictionary:command
															   error:&serializeError];
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
			UnitySendMessage("AppMetrCommandListener", "OnExecuteCommand", [serializedData UTF8String]);
		}
	}
}

@end
