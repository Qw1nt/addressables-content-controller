# Addressables Content Controller
Удобное управление контентом

## Содержание 
* [Установка](#установка-)
* [Основное](#основное)
* [Интеграция с VContainer](#интеграция-с-vcontainer)

## 📖 Установка 

* ### Unity-модуль
Поддерживается установка в виде Unity-модуля в при помощи добавления git-URL в [PackageManager](https://docs.unity3d.com/2023.2/Documentation/Manual/upm-ui-giturl.html) или ручного добавления в ``Packages/manifest.json``:

```
  https://github.com/Qw1nt/addressables-content-controller.git
```


## 📖 Основное

### Функицонал:
1) Механизм загрузки и автоматичнской выгрузки ресурсов в рамках сцены,
2) Кеширование загружаемых активов,
3) Предзагрузка сцен,
4) Реактивное отслеживание состояния загрузки.


## Интеграция с VContainer

Для включения интеграции с VContainer достаточно добавить ``CONTENT_CONTROLLER_VCONTAINER`` в [scripting define symbols](https://docs.unity3d.com/Manual/CustomScriptingSymbols.html)

### Создание объектов

| Класс                 | Метод          | Параметры                        |
|:----------------------|:---------------|:---------------------------------|
| **ContentController** | CreateInstance | **IObjectResolver** DI контейнер |

