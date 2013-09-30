Класс AppmetrPlugin

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
_event - название события
properties - дополнительные параметры


- TrackPayment
Регистрирует совершение платежа

Параметры:
payment - сведения о выполненной покупке
properties - дополнительные параметры


Класс AppMetrCommandListener

Этот класс предоставляет доступ к событию получения команды от сервера.

Пример использования:

AppMetrCommandListener.OnCommand += HandleOnCommand; 

public void HandleOnCommand(string command)
{
	Debug.Log("HandleOnCommand:\n" + command);
}

Пример использования плагина можно посмотреть в Plugins/Appmetr/Sample/AppMetrSample.cs

ВАЖНО: Необходимо добавить игровой объект на сцену, назвав его "AppMetrCommandListener", 
 и подключить к объекту скрипт AppMetrCommandListener.cs.
