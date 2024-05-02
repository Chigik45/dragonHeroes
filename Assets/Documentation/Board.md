## Board
> 
##### Inherits from:
 - System.Object
---
### Fields
```cs
Tile[,] tiles
```
> Гескагональное поле 20 на 20 двумерным массивом объектов Tile
```cs
List`1 entities
```
> Лист сущностей (объектов, персонажей) листом объектов Entity

### Methods
```cs
ValueTuple`2 GetTileCoordinates
```
> Метод GetTileCoordinates который возвращает (кортежем х и у) координаты ссылки на Tile в масиве tiles
```cs
Point GetTileCoordinates
```
> Метод GetTileCoordinates который возвращает (объектом Point) координаты ссылки на Tile в масиве tiles
```cs
Tile GetTile
```
> Метод GetTile который возвращает ссылку на Tile по координатам в масиве tiles
```cs
List`1 Near
```
> Метод Near который возвращает все ближайшие к выбранному тайлы в массиве tiles как лист ссылок на Tile
```cs
List`1 NearAndEmpty
```
> Метод NearAndEmpty который возвращает все ближайшие к выбранному тайлы в массиве tiles, которые не заняты сущностями, как лист ссылок на Tile
```cs
List`1 GetPathToEnemy
```
> Метод GetPathToEnemy который возвращает лист ссылок на Tile (путь, который необходимо пройти для подхождения вплотную к той сущности, которая приходится врагом сущности из параметра)
```cs
Entity GetNearestEnemy
```
> Метод GetNearestEnemy который возвращает ближайшего врага для сущности из параметра в виде ссылки на Entity
```cs
Entity GetEntityOnTile
```
> Метод GetEntityOnTile возвращает объект Entity если на тайле в параметре кто-то стоит, и null если не стоит никто
```cs
Int32 CheckDeads
```
> Метод CheckDeads удаляет сущности, отмеченные флагом isDead, и возращает количество сущностей, умерших в этом раунде

