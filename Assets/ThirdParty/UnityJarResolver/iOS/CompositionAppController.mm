//
//  CompositionAppController.mm
//  UnityJarResolver extension
//
//  Created by Andrey Gruzinov on 09.02.18.
//  Copyright © 2018 Pixonic. All rights reserved.
//

#import "UnityAppController.h"
#import "AppDelegateListener.h"
#import "CompositionAppController.h"

#define SEND_INVOKATION_TO_COMPONENTS(InvokationSelector) \
    for (id<UIApplicationDelegate> delegate in _applicationDelegates) { \
        if([delegate respondsToSelector:_cmd]) \
            [delegate InvokationSelector];\
    } \

#define SEND_NOTIFICATION_SELECTOR(ApplicationSelector) \
    for (id<UIApplicationDelegate> delegate in _applicationDelegates) { \
        if([delegate respondsToSelector:@selector(ApplicationSelector)]) \
            [delegate ApplicationSelector [UIApplication sharedApplication]]; \
    } \


@interface CompositionAppController : UnityAppController <AppDelegateListener>
@end

@implementation CompositionAppController

static NSMutableArray<UIApplicationDelegate>* _applicationDelegates;
NSInteger _forceSupportedCompositionInterfaceOrientationMask = 0;

-(instancetype)init {
    if(_applicationDelegates == nil)
        _applicationDelegates = [NSMutableArray<UIApplicationDelegate> array];
    self = [super init];
    if(self) {
        UnityRegisterAppDelegateListener(self);
    }
    return self;
}

-(void)dealloc {
    UnityUnregisterAppDelegateListener(self);
}

#pragma mark UIApplicationDelegate

- (NSUInteger)application:(UIApplication*)application supportedInterfaceOrientationsForWindow:(UIWindow*)window
{
    // No rootViewController is set because we are switching from one view controller to another, all orientations should be enabled
    if ([window rootViewController] == nil)
        return UIInterfaceOrientationMaskAll;
    
    // By default we return settigs from Info.plist
    NSArray *supportedOrientations = [[[NSBundle mainBundle] infoDictionary] objectForKey:@"UISupportedInterfaceOrientations"];
    if(supportedOrientations == nil)
        throw @"UISupportedInterfaceOrientations not found in Info.plist";
    NSUInteger orientationMask = 0;
    for (NSString* orientation in supportedOrientations) {
        if([orientation isEqualToString:@"UIInterfaceOrientationLandscapeLeft"])
            orientationMask |= UIInterfaceOrientationMaskLandscapeLeft;
        else if([orientation isEqualToString:@"UIInterfaceOrientationLandscapeRight"])
            orientationMask |= UIInterfaceOrientationMaskLandscapeRight;
        else if([orientation isEqualToString:@"UIInterfaceOrientationPortrait"])
            orientationMask |= UIInterfaceOrientationMaskPortrait;
        else if([orientation isEqualToString:@"UIInterfaceOrientationPortraitUpsideDown"])
            orientationMask |= UIInterfaceOrientationMaskPortraitUpsideDown;
    }
    // Some presentation controllers (e.g. UIImagePickerController) require portrait orientation and will throw exception if it is not supported.
    // At the same time enabling all orientations by returning UIInterfaceOrientationMaskAll might cause unwanted orientation change
    // (e.g. when using UIActivityViewController to "share to" another application, iOS will use supportedInterfaceOrientations to possibly reorient).
    // So to avoid exception we are returning combination of constraints for root view controller and orientation requested by iOS.
    // _forceSupportedCompositionInterfaceOrientationMask is updated in willChangeStatusBarOrientation, which is called if some presentation controller insists on orientation change.
    return orientationMask | _forceSupportedCompositionInterfaceOrientationMask;
}

- (void)application:(UIApplication*)application willChangeStatusBarOrientation:(UIInterfaceOrientation)newStatusBarOrientation duration:(NSTimeInterval)duration
{
    // Setting orientation mask which is requested by iOS: see supportedInterfaceOrientationsForWindow above for details
    _forceSupportedCompositionInterfaceOrientationMask = 1 << newStatusBarOrientation;
}

-(BOOL)application:(UIApplication *)application didFinishLaunchingWithOptions:(NSDictionary *)launchOptions {
    BOOL result = [super application:application didFinishLaunchingWithOptions:launchOptions];
    SEND_INVOKATION_TO_COMPONENTS(application:application didFinishLaunchingWithOptions:launchOptions)
    return result;
}

- (void)application:(UIApplication *)application didRegisterUserNotificationSettings:(UIUserNotificationSettings *)notificationSettings {
    SEND_INVOKATION_TO_COMPONENTS(application:application didRegisterUserNotificationSettings:notificationSettings)
}

-(BOOL)application:(UIApplication *)application openURL:(NSURL *)url options:(NSDictionary *)options {
    SEND_INVOKATION_TO_COMPONENTS(application:application openURL:url options:options)
    if (@available(iOS 9.0, *)) {
        NSString* sourceApplication = [options valueForKey:UIApplicationOpenURLOptionsSourceApplicationKey];
        id annotation = [options valueForKey:UIApplicationOpenURLOptionsAnnotationKey];
        [self application:application openURL:url sourceApplication:sourceApplication annotation:annotation];
        if([UnityAppController instancesRespondToSelector:_cmd])
            [super application:application openURL:url options:options];
    }
    return YES;
}

- (BOOL) application:(UIApplication *)application openURL:(nonnull NSURL *)url sourceApplication:(nullable NSString *)sourceApplication annotation:(nonnull id)annotation {
    SEND_INVOKATION_TO_COMPONENTS(application:application openURL:url sourceApplication:sourceApplication annotation:annotation)
    if([UnityAppController instancesRespondToSelector:_cmd])
        [super application:application openURL:url sourceApplication:sourceApplication annotation:annotation];
    return YES;
}

- (BOOL)application:(UIApplication *)application continueUserActivity:(NSUserActivity *)userActivity restorationHandler:
#if defined(__IPHONE_12_0) && (__IPHONE_OS_VERSION_MAX_ALLOWED >= __IPHONE_12_0)
(nonnull void (^)(NSArray<id<UIUserActivityRestoring>> *_Nullable))restorationHandler
#else
(nonnull void (^)(NSArray *_Nullable))restorationHandler
#endif  // __IPHONE_12_0
{
    SEND_INVOKATION_TO_COMPONENTS(application:application continueUserActivity:userActivity restorationHandler:restorationHandler)
    return YES;
}

- (void)application:(UIApplication *)application handleEventsForBackgroundURLSession:(NSString *)identifier completionHandler:(void (^)(void))completionHandler {
    SEND_INVOKATION_TO_COMPONENTS(application:application handleEventsForBackgroundURLSession:identifier completionHandler:completionHandler)
}

- (void)application:(UIApplication *)application performActionForShortcutItem:(UIApplicationShortcutItem*)shortcutItem completionHandler:(void (^)(BOOL succeeded))completionHandler {
    SEND_INVOKATION_TO_COMPONENTS(application:application performActionForShortcutItem:shortcutItem completionHandler:completionHandler)
}

- (void)applicationDidReceiveMemoryWarning:(UIApplication*)application
{
    if([UnityAppController instancesRespondToSelector:_cmd])
        [super applicationDidReceiveMemoryWarning:application];
    SEND_NOTIFICATION_SELECTOR(applicationDidReceiveMemoryWarning:)
}

#pragma mark AppDelegateListener

- (void)didReceiveRemoteNotification:(NSNotification*)notification {
    NSDictionary* userInfo = notification.userInfo;
    for (id<UIApplicationDelegate> delegate in _applicationDelegates) {
        if([delegate respondsToSelector:@selector(application:didReceiveRemoteNotification:)])
            [delegate application:[UIApplication sharedApplication] didReceiveRemoteNotification:userInfo];
    }
}

- (void)didReceiveLocalNotification:(NSNotification*)notification {
    UILocalNotification* localNotification = (UILocalNotification*)notification.userInfo;
    for (id<UIApplicationDelegate> delegate in _applicationDelegates) {
        if([delegate respondsToSelector:@selector(application:didReceiveLocalNotification:)])
            [delegate application:[UIApplication sharedApplication] didReceiveLocalNotification:localNotification];
    }
}

- (void)didRegisterForRemoteNotificationsWithDeviceToken:(NSNotification*)notification {
    NSData* deviceToken = (NSData*)notification.userInfo;
    for (id<UIApplicationDelegate> delegate in _applicationDelegates) {
        if([delegate respondsToSelector:@selector(application:didRegisterForRemoteNotificationsWithDeviceToken:)])
            [delegate application:[UIApplication sharedApplication] didRegisterForRemoteNotificationsWithDeviceToken:deviceToken];
    }
}

- (void)didFailToRegisterForRemoteNotificationsWithError:(NSNotification*)notification {
    NSError* error = (NSError*)notification.userInfo;
    for (id<UIApplicationDelegate> delegate in _applicationDelegates) {
        if([delegate respondsToSelector:@selector(application:didFailToRegisterForRemoteNotificationsWithError:)])
            [delegate application:[UIApplication sharedApplication] didFailToRegisterForRemoteNotificationsWithError:error];
    }
}

- (void)applicationSignificantTimeChange:(NSNotification*)notification { SEND_NOTIFICATION_SELECTOR(applicationSignificantTimeChange:) }

#pragma mark LifeCycleListener

- (void)didBecomeActive:(NSNotification*)notification { SEND_NOTIFICATION_SELECTOR(applicationDidBecomeActive:) }
- (void)willResignActive:(NSNotification*)notification { SEND_NOTIFICATION_SELECTOR(applicationWillResignActive:) }
- (void)didEnterBackground:(NSNotification*)notification { SEND_NOTIFICATION_SELECTOR(applicationDidEnterBackground:) }
- (void)willEnterForeground:(NSNotification*)notification { SEND_NOTIFICATION_SELECTOR(applicationWillEnterForeground:) }
- (void)willTerminate:(NSNotification*)notification { SEND_NOTIFICATION_SELECTOR(applicationWillTerminate:) }

@end

FOUNDATION_EXPORT void AddApplicationDelegateComponent(id<UIApplicationDelegate> appDelegate) {
    if(_applicationDelegates == nil)
        _applicationDelegates = [NSMutableArray<UIApplicationDelegate> array];
    if(appDelegate != nil && ![_applicationDelegates containsObject:appDelegate]) {
        [_applicationDelegates addObject:appDelegate];
    }
}

IMPL_APP_CONTROLLER_SUBCLASS(CompositionAppController)

#pragma mark StubController

@implementation StubController

@synthesize window;

-(UIWindow *)window {
    UIWindow *keyWindow = [[UIApplication sharedApplication] keyWindow];
    if (keyWindow == nil) {
        keyWindow = [[[UIApplication sharedApplication] windows] firstObject];
    }
    return keyWindow;
}

-(void)setWindow:(UIWindow *)window {}

- (NSUInteger)application:(UIApplication*)application supportedInterfaceOrientationsForWindow:(UIWindow*)window {
    return UIInterfaceOrientationMaskAll;
}
- (void)application:(UIApplication*)application didReceiveLocalNotification:(UILocalNotification*)notification {}
- (void)application:(UIApplication*)application didReceiveRemoteNotification:(NSDictionary*)userInfo {}
- (void)application:(UIApplication*)application didRegisterForRemoteNotificationsWithDeviceToken:(NSData*)deviceToken {}
- (void)application:(UIApplication *)application didReceiveRemoteNotification:(NSDictionary *)userInfo fetchCompletionHandler:(void (^)(UIBackgroundFetchResult result))handler {}
- (void)application:(UIApplication*)application didFailToRegisterForRemoteNotificationsWithError:(NSError*)error {}
- (BOOL)application:(UIApplication*)application openURL:(NSURL*)url sourceApplication:(NSString*)sourceApplication annotation:(id)annotation {
    return YES;
}
-(BOOL)application:(UIApplication *)application openURL:(NSURL *)url options:(NSDictionary *)options {
    return YES;
}
- (BOOL)application:(UIApplication*)application willFinishLaunchingWithOptions:(NSDictionary*)launchOptions {
    return YES;
}
- (BOOL)application:(UIApplication*)application didFinishLaunchingWithOptions:(NSDictionary*)launchOptions {
    return YES;
}
- (void)applicationDidEnterBackground:(UIApplication*)application {}
- (void)applicationWillEnterForeground:(UIApplication*)application {}
- (void)applicationDidBecomeActive:(UIApplication*)application {}
- (void)applicationWillResignActive:(UIApplication*)application {}
- (void)applicationDidReceiveMemoryWarning:(UIApplication*)application {}
- (void)applicationWillTerminate:(UIApplication*)application {}

@end
