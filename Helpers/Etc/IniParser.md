## 初始化
```csharp
string configpath = AppDomain.CurrentDomain.BaseDirectory + "config.ini";
IniParser parser = IniParser.Load(configpath);
```
通过路径加载配置文件，如果文件不存在，会创建一个新的配置文件

## 写入/删除配置

Write(string section, string key, string value)

Delete(string section, string key)
```csharp
//成功返回1
long success=parser.Write("setting", "port", "465");
long deleted=parser.Delete("setting", "port");
```
在写入配置的时候需要节点名、配置名和值

其中，相同节点下配置名是唯一的，如果配置名已存在，写入的时候会覆盖原来的配置

而相同的配置名可以在不同的节点下

## 获取所有的节点名
```csharp
string[] sections= parser.GetSections();
```

## 获取指定节点的所有配置
```csharp
Dictionary<string, string> configs = parser.GetKeyValues("Setting");
```

## 获取指定节点的指定配置
```csharp
string port = parser.ReadString("setting", "port");
```