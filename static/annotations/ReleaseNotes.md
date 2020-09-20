Version History
===============

### 1.1.5

* Adding new attribute [IgnoreParameter](https://github.com/rgarrison12345/ADO.Net.Client/blob/master/src/ADO.Net.Client.Annotations/IgnoreParameter.cs) to indicate that an instance of [DbParameter](https://docs.microsoft.com/en-us/dotnet/api/system.data.common.dbparameter?view=netcore-3.1) should not be created from a property

### 1.1.4

* Adding new attribute [ParameterName](https://github.com/rgarrison12345/ADO.Net.Client/blob/master/src/ADO.Net.Client.Annotations/ParameterName.cs) to specify alternate parameter name when creating an instance of [DbParameter](https://docs.microsoft.com/en-us/dotnet/api/system.data.common.dbparameter?view=netcore-3.1)

### 1.1.3

* Adding attribute [Input](https://github.com/rgarrison12345/ADO.Net.Client/blob/master/src/ADO.Net.Client.Annotations/Input.cs) class to indicate an input database parameter
* Adding attribute [Output](https://github.com/rgarrison12345/ADO.Net.Client/blob/master/src/ADO.Net.Client.Annotations/Output.cs) to indicate an output database parameter
* Adding attribute [InputOutput](https://github.com/rgarrison12345/ADO.Net.Client/blob/master/src/ADO.Net.Client.Annotations/InputOutput.cs) to indicate an input output database parameter
* Adding attribute [ReturnValue](https://github.com/rgarrison12345/ADO.Net.Client/blob/master/src/ADO.Net.Client.Annotations/ReturnValue.cs) to indicate a return value database parameter

### 1.1.2

* Correcting DateTim2 attribute to be DateTime2

### 1.1.1

* Adding new annotation
  * [DateTime2](https://github.com/rgarrison12345/ADO.Net.Client/blob/master/src/ADO.Net.Client.Annotations/DateTim2.cs)
    * Signifies data type is equivelant of DateTime2

### 1.1.0

* Adding .NET 4.5 and .NET 4.6.1 builds
* Adding three new annotations
  * [AnsiString](https://github.com/rgarrison12345/ADO.Net.Client/blob/master/src/ADO.Net.Client.Annotations/AnsiString.cs)
    * Signifies data type is equivelant of varchar
  * [AnsiStringFixedLength](https://github.com/rgarrison12345/ADO.Net.Client/blob/master/src/ADO.Net.Client.Annotations/ANSIStringFixedLength.cs)
    * Signifies data type is equivelant of char
  * [StringFixedLength](https://github.com/rgarrison12345/ADO.Net.Client/blob/master/src/ADO.Net.Client.Annotations/StringFixedLength.cs)
    * Signifies data type is equivelant of ncar
### 1.0.0

* Initial release, this is a data annotation library to help annotate data types that have an ambiguous match of DbType.  Items such as String and DateTime can match several DbType values.
  * [DbField](https://github.com/rgarrison12345/ADO.Net.Client/blob/master/src/ADO.Net.Client.Annotations/DbField.cs)
  * [DbFieldIgnore](https://github.com/rgarrison12345/ADO.Net.Client/blob/master/src/ADO.Net.Client.Annotations/DbFieldIgnore.cs)
  * [PrimaryKey](https://github.com/rgarrison12345/ADO.Net.Client/blob/master/src/ADO.Net.Client.Annotations/PrimaryKey.cs)