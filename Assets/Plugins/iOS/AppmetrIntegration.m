#import "AppmetrIntegration.h"
#import "AppMetr.h"
#import "CJSONSerializer.h"

#ifndef SANDBOX_MODE
#	define SANDBOX_MODE 0
#endif // SANDBOX_MODE

#if(SANDBOX_MODE)
// stage key
NSString * const kAppMetrToken = @"c0ceb276-34be-4340-844d-67a23054f708";
#else
// release key
NSString * const kAppMetrToken = @"29cbd610-de0f-4bea-ae31-c9c192d006a1";
#endif

@implementation AppmetrIntegration
{

}

-(id)init
{
	self = [super init];
	if(self)
	{
		// setting to AppMetr
		[AppMetr setupWithToken:kAppMetrToken andDelegate:self];
	}
	return self;
}

-(void)dealloc
{
	[[AppMetr sharedInstance] setDelegate:nil];
	[super dealloc];
}


#pragma mark - AppMetrDelegate

-(void)executeCommand:(NSDictionary *)command
{
	NSError *serializeError = nil;
	NSData *data = [[CJSONSerializer serializer] serializeDictionary:command
															   error:&serializeError];
	if(serializeError)
	{
		NSLog(@"JSON srializer error: %@", serializeError.description);
		[NSException raise:NSGenericException
					format:@"%@", serializeError.description];
	}
	else
	{
		NSString *serializedData = [[[NSString alloc] initWithData:data encoding:NSUTF8StringEncoding] autorelease];
		if(serializedData)
		{
			// .. onExecuteCommand([serializedData UTF8String]);
		}
	}
}

@end
