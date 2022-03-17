//
//  CompositionAppController.h
//  UnityJarResolver extension
//
//  Created by Andrey Gruzinov on 09.02.2018.
//

#ifndef CompositionAppController_h
#define CompositionAppController_h

#import <UIKit/UIKit.h>

FOUNDATION_EXPORT void AddApplicationDelegateComponent(id<UIApplicationDelegate> appDelegate);

// Put this into mm file with your UIApplicationController delegate
// pass subclass name to define

#define ADD_APPLICATION_DELEGATE_COMPONENT(ClassName)   \
@interface ClassName(ApplicationDelegateComponent)      \
+(void)load;                                            \
@end                                                    \
@implementation ClassName(ApplicationDelegateComponent) \
+(void)load                                             \
{                                                       \
 AddApplicationDelegateComponent([[self alloc] init]);  \
}                                                       \
@end                                                    \

@interface StubController : NSObject<UIApplicationDelegate>

- (NSUInteger)application:(UIApplication*)application supportedInterfaceOrientationsForWindow:(UIWindow*)window;
- (void)application:(UIApplication*)application didReceiveLocalNotification:(UILocalNotification*)notification;
- (void)application:(UIApplication*)application didReceiveRemoteNotification:(NSDictionary*)userInfo;
- (void)application:(UIApplication*)application didRegisterForRemoteNotificationsWithDeviceToken:(NSData*)deviceToken;
- (void)application:(UIApplication *)application didReceiveRemoteNotification:(NSDictionary *)userInfo fetchCompletionHandler:(void (^)(UIBackgroundFetchResult result))handler;
- (void)application:(UIApplication*)application didFailToRegisterForRemoteNotificationsWithError:(NSError*)error;
- (BOOL)application:(UIApplication*)application openURL:(NSURL*)url sourceApplication:(NSString*)sourceApplication annotation:(id)annotation;
- (BOOL)application:(UIApplication*)application willFinishLaunchingWithOptions:(NSDictionary*)launchOptions;
- (BOOL)application:(UIApplication*)application didFinishLaunchingWithOptions:(NSDictionary*)launchOptions;
- (void)applicationDidEnterBackground:(UIApplication*)application;
- (void)applicationWillEnterForeground:(UIApplication*)application;
- (void)applicationDidBecomeActive:(UIApplication*)application;
- (void)applicationWillResignActive:(UIApplication*)application;
- (void)applicationDidReceiveMemoryWarning:(UIApplication*)application;
- (void)applicationWillTerminate:(UIApplication*)application;

@end

#endif /* CompositionAppController_h */
