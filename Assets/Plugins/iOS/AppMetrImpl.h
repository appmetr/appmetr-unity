#import <Foundation/Foundation.h>
#import <AppMetr.h>

@interface AppMetrImpl : NSObject
{
	NSMutableDictionary *keyValueDict_;
	NSMutableDictionary *keyValueDictOptional_;
}

@property (nonatomic, retain) NSMutableDictionary *keyValueDict;
@property (nonatomic, retain) NSMutableDictionary *keyValueDictOptional;

+ (AppMetrImpl*)sharedAppMetrImpl;

- (void)setKey:(NSString*)key Value:(NSString*)value;
- (void)setKeyOptional:(NSString*)key Value:(NSString*)value;
- (void)resetDict;

@end
