#import <Foundation/Foundation.h>
#import "AppMetr.h"
#import "AppMetrListener.h"

@interface AppMetrImpl : NSObject
{
	NSMutableDictionary *keyValueDict_;
	NSMutableDictionary *keyValueDictOptional_;
	
	AppMetrListener *appMetrListener_;
}

@property (nonatomic, retain) NSMutableDictionary *keyValueDict;
@property (nonatomic, retain) NSMutableDictionary *keyValueDictOptional;
@property (nonatomic, readonly) AppMetrListener *appMetrListener;

+ (AppMetrImpl*)sharedAppMetrImpl;

- (void)setKey:(NSString*)key Value:(NSString*)value;
- (void)setKeyOptional:(NSString*)key Value:(NSString*)value;
- (void)resetDict;

@end
