# Using the API

### Overview
This is an API for managing a player character's Hit Points (HP) within DnD. The API enables clients to perform various operations related to HP, including dealing damage of different types, considering character resistances and immunities, healing, and adding temporary Hit Points.

### Instructions to Run Locally
1. Clone the repository or obtain the project files. You can do this using Git by running the following command in your terminal or command prompt:
   ```bash
   git clone https://github.com/nathantinius/Hasbro.git
   
2. Navigate to the Project Directory:
   ```bash
   cd Hasbro
   
3. Install .NET SDK:\
   Ensure you have the .NET SDK installed on your system. You can download it from the official .NET website if you haven't already.

4. Run the Database: (You will need Docker)
   ```bash
   cd src/HitPoints.Application/
   docker compose up

5. Open a new commandline and CD into the API Project
   ```bash
   cd src/HitPoints.Api/

6. Restore Dependencies:
   ```bash
   dotnet restore
   
7. Build the Project:
   ```bash
   dotnet build
   
8. Run the Application:
   ```bash
   dotnet run

EndPoints
------------------------------------------------------------------------------------------

#### Updating a Player Characters Hit Points

<details>
 <summary><code>PUT</code> <code><b>/</b></code> <code>(Adds damage, health, or temporary hit points to the provided player character)</code></summary>

##### Parameters

> | name       | type     | data type | description                                                                                                                                                                                |
> |------------|----------|-----------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
> | name       | required | string    | The name of the player character being impacted                                                                                                                                            |
> | action     | required | string    | damage, heal, or temporary                                                                                                                                                                 |
> | value      | required | int       | The amount of damage, healing, or temporary hit points                                                                                                                                     |
> | damageType | optional | string    | When the action is damage this parameter is required. Valid types are: Bludgeoning, Piercing, Slashing, Fire, Cold, Acid, Thunder, Lightning, Poison, Radiant, Necrotic, Psychic, or Force |


##### Responses

> | http code | content-type              | response                                               |
> |-----------|---------------------------|--------------------------------------------------------|
> | `200`     | `application/json`        | `{message: String, playerCharacter: PlayerCharacter}`  |
> | `400`     | `application/json`        | `Errors: [{"PropertyName":String, "message": String}]` |

</details>





