AppMetr Unity Plugin

Интеграция плагина

1. Импортируете плагин *AppmetrUnityPlugin-version.unitypackage* в проект (двойным кликом по файлу или через меню *Assets/Import Package/Custom Package…*).

2. Добавьте на сцену скрипт Assets/ThirdParty/AppMetr/AppMetrBehaviour.cs и не выгружайте его при перезагрузке сцены (можно для этого установить флаг Single Unloadable Instance в его свойствах). Пропишите свойство Token

3. Вызывайте статические методы AppMetr для регистрации игровых событий (см. AppMetr API)

4. Опционально. Подписываетесь на события AppMetrCommandListener.OnCommand, чтобы выполнять удаленные серверные вызовы. Пример кода:

using UnityEngine;
using System.Collections;
using Appmetr.Unity;

public class AppMetrManager : MonoBehaviour {

	void Awake() {
		AppMetrBehaviour.OnCommand += HandleAppMetrCommand; 
	}
	
	void OnDisable() {
		AppMetrBehaviour.OnCommand -= HandleAppMetrOnCommand;
	
	public void HandleAppMetrCommand(string commandJson) {
		Debug.Log("AppMetrManager: HandleAppMetrCommand\n" + commandJson);
	}
}

AppMetr API

Класс AppMetrBehaviour

Служит для прослушивания основных событий системы, предоставляемых MonoBehaviour, и передачи их в AppMetr. Должен присутствовать на сцене и не выгружаться в течение всей игровой сессии

Класс AppMetr

Это основной класс, предоставляющий доступ к функциям библиотеки.
Методы:
- Setup
Инициализирует библиотеку
Параметры:
token - токен приложения

- TrackSession
Регистрирует запуск приложения
Параметры:
properties - дополнительные параметры (сведения о пользователе, например, дата регистрации, имя, возраст)

- TrackLevel
Регистрирует достижения нового уровня игроком
Параметры:
level - достигнутый уровень
properties - дополнительные параметры

- TrackEvent
Регистрирует игровое событие
Параметры:
eventName - название события
properties - дополнительные параметры

- TrackPayment
Регистрирует совершение платежа
Параметры:
payment - сведения о выполненной покупке
properties - дополнительные параметры

- AttachProperties
Добавление параметров
Параметры:
properties - параметры

- Identify
Идентификация пользователя
Параметры:
userId - идентификатор пользователя

- Flush
Форсированная отправка событий на сервер

Пример использования плагина можно посмотреть в *Assets/ThirdParty/Appmetr/Sample/TestScene.unity*.

ВАЖНО:
Библиотека не поддерживает Stripping Level = use micro mscorlib.
Все функции кроме *AppMetr.Setup* потокобезопасны.
