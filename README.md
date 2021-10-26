# BattleShipApi
API for BattleShip game - 
<br />
Swagger link - <https://battle-ship-web-api.azurewebsites.net/index.html>

## Usage
git clone
<br />
Runs in Visual Studio 2019

Run tests -
<br />
dotnet test

Endpoints
<br />
### 1. POST /board -> Creates a board with default configuration for any gameID, playerID, colorPreference.

##### Color
|            | ID         | 
| ------------- |:-------------:| 
| Red           | 0             | 
| Blue          | 1             |   

Sample Response with default configurations
```json

{
  "boardID": "9c35def1-1d93-425f-a869-f9b75a54ea6f",
  "gameID": 0,
  "playerID": 0,
  "maxRows": 10,
  "maxNumberOfShips": 5,
  "canOverLap": false,
  "color": 0
}
```

Use the boardID from response on subsequent requests

### 2. POST /board/battleShip/{boardID} -> Places battleship on board

##### Battleshiptype
||ID|Length| 
| ------------- |:-------------:| ------------|
|Carrier|1|1| 
|BattleShip|2|2|
|Destroyer|3|3|
|Submarine|4|4|
|PatrolBat|5|5|

##### BattleShipAllignment
|            | ID         | 
| ------------- |:-------------:| 
| Horizontal           | 0             | 
| Vertical          | 1          |   


### 3. POST /board/attack/{boardID} -> Attacks the the target cell on board

##### AttackResponse
|               |ID             | 
| ------------- |:-------------:| 
| Miss          | 1             | 
| Hit           | 0             |   





