Setup Development Environment Tool
=============================

Проект предназначен для удобного и быстрого развертывания и настройки проекта bpmonline 7.12.0 с исходников ядра и конфигурационных пакетов

Что делает утилита:
* Выкачивает и настраивает исходные коды ядра 
* Разворачивает и настраивает бд приложения
* Выкачивает и настраивает пакеты конфигурации
* Компилирует ядро
* Генерирует исходные коды и компилирует конфигурацию
* Запускает проект студии


INSTALLATION
---------------------
### Setup
Скачать последнюю релизную версию. [Release](https://github.com/Advance-Technologies-Foundation/SetupDevEnv/releases)

Сконфигурировать программу. [примеры конфигов](http://tswiki/x/QQd-B)

Запустить и следовать инструкциям.

>время работы утилиты примерно от 15 до 25 минут в зависимости от количества пакетов

### Config file

Установка значений в config файле приложения

##### MSSSQLConnectionString
Строка подключения к базе данных
>Макрос ##dbname## будет заменен на имя базы из свойства ***DatabaseNamePattern***

```xml
<add key="MSSSQLConnectionString" value="Data Source = localhost; Initial Catalog = ##dbname##; Persist Security Info = True; MultipleActiveResultSets = True; Integrated Security = SSPI; Pooling = true; Max Pool Size = 100; Async = true; Connection Timeout = 500" />
```
##### SvnUserName
Имя пользователя в SVN
```xml
<add key="SvnUserName" value="I.Ivanov" />
```
##### SvnUserPassword
Пароль пользователя в SVN
```xml
<add key="SvnUserPassword" value="*****" />
```
##### Packages
Перечень пакетов в которые планируются вносить изменения. Указанный перечень пакетов будет доступен для изменения в системе и заливки в SVN
```xml
<add key="Packages" value="Base,Core" />
```
> при указании * будут выкачаны все пакеты установленные в бд.

##### JSUnitTestsPackages & DBUnitTestsPackages
Перечень пакетов с тестами javascript и пакетов с тестами  SQL соответсвенно в которые планируются вносить изменения. Указанный перечень пакетов будет доступен для изменения в системе и заливки в SVN
```xml
<add key="JSUnitTestsPackages" value="*" />
<add key="DBUnitTestsPackages" value="*" />
<!-- OR -->
<add key="JSUnitTestsPackages" value="" />
<add key="DBUnitTestsPackages" value="" />
```
> при указании * будут выкачаны все соответствующие пакеты тестов которые соответствуют установленным пакетам. При пустом значении шаг будет проигнорирован

##### CSUnitTestsProjects
Перечень C# проектов с тестами конфигурации.
```xml
    <add key="CSUnitTestsProjects" value="*" />
```
##### ProjectsPath
Путь к каталогу в который будет выгружен проект
```xml
    <add key="ProjectsPath" value="C:\Projects\AutoCheckout" />
```
##### DfsBuildsDirectoryPath
Путь к хранилищу сборок (dfs)
```xml
    <add key="DfsBuildsDirectoryPath" value="\\dfs\build" />
```

##### InfrastructureConsolePath
Путь к хранилищу где лежит инфраструктурная консоль
```xml
<add key="InfrastructureConsolePath" value="http://infr-path" />
```
##### UnitTestsPath
Путь к хранилищу C# тестов конфигурации
```xml
<add key="UnitTestsPath" value="http://unit-path" />
```
##### TSqltPath
Путь к хранилищу tSqlT установщик
```xml
<add key="TSqltPath" value="http://tsqlt-path" />
```
##### CorePath & PackageStorePath
Путь к исходникам ядра и пакетов конфигурации соответсвенно
```xml
<add key="CorePath" value="http://infr-path" />
 <add key="PackageStorePath" value="http://package-store" />
```
##### DatabaseNamePattern
Шаблон именования базы данных используется при разворачивании бд.
```xml
<add key="DatabaseNamePattern" value="DEV_##username##_##projectname##" />
```
##### SharedDirectoryPath
Сетевая директория пользователя с правом на запись.
```xml
<add key="SharedDirectoryPath" value="\\User\Share" />
```
##### ClearProjectDirectory
Признак очистки директории проекта, если установлен в true директория с исходниками будет очищена если она не пустая
```xml
    <add key="ClearProjectDirectory" value="true" />
```
##### ClearSvnAuthenticationCache
Признак очистки svn кеша с сохраненными авторизационными данными
```xml
<add key="ClearSvnAuthenticationCache" value="false" />
```
##### UpdateWorkspace
Признак выполнения команды UpdateWorkspace. Если установлен в true то будет выполнена команда генерации исходных кодов конфигурации.
```xml
<add key="UpdateWorkspace" value="true" />
```
##### BuildWorkspace
Признак выполнения команды BuildWorkspace. Если установлен в true то будет выполнена команда компиляции конфигурации.
```xml
<add key="BuildWorkspace" value="true" />
```
### Последовательность работы:
