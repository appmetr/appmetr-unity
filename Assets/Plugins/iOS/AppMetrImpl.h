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

- (void)setKeyString:(NSString*)key Value:(NSString*)value;
- (void)setKeyNumber:(NSString*)key Value:(NSNumber*)value;
- (void)setKeyStringOptional:(NSString*)key Value:(NSString*)value;
- (void)setKeyNumberOptional:(NSString*)key Value:(NSNumber*)value;
- (void)resetDict;

@end
