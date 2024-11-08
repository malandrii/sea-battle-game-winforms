# "Sea Battle" on Windows Forms
Classic "Sea Battle" game on Windows Forms with Enemy bot.

## Installation
Go to <a href="https://github.com/malandrii/sea-battle-game-winforms/releases">Releases</a>, choose the latest release and click the "sea-battle.exe" or
just click <a href="https://github.com/malandrii/sea-battle-game-winforms/releases/download/v1.3/sea-battle.exe">here</a>.

## Usage
### 1. Place your ships
> *Place ships panel* => *choose ship size* => *mouse cursor on the field*

![image](https://github.com/user-attachments/assets/e7eac021-077c-4310-a52e-724a25f59517)

> *mouse left button to place a ship*

![image](https://github.com/user-attachments/assets/fffe251c-c249-4dea-af9b-2dae4268b0cf)

> Ships horizontality can be changed <br /> <br />
![image](https://github.com/user-attachments/assets/3cb9109d-efa9-4bdc-bddf-9934e0db9697) <br />
![image](https://github.com/user-attachments/assets/1706d821-1818-41ec-b295-b3c3e1c5133e) <br />

> Ships can be arranged randomly <br /> <br />
![image](https://github.com/user-attachments/assets/a58e8b77-4171-46ef-bd2d-4a87c37ee831)

### 2. Start Game
After you've placed all the ships, choose the enemy settings and press "Start Game".

![image](https://github.com/user-attachments/assets/96950755-84dd-453e-90a1-38d63dcde53a)

### 3. Your turns
* Press on the enemy field button. You and the enemy go one by one. <br />
* The text of the button displays the turn outcome. `dot` is a hit, `cross` is a miss. <br />

![image](https://github.com/user-attachments/assets/d1a34138-4dab-4971-adec-48d148c0c4b3)

`Status` also helps you monitoring current move action.

![image](https://user-images.githubusercontent.com/111363234/205210209-139caa90-2075-4448-ad82-0b44f22a760d.png)

* When you hit an enemy ship you will be able to continue your moves until you miss. <br />
* When the enemy ship becomes dead - all the cells around it get covered in dots.

![image](https://github.com/user-attachments/assets/e0f3735b-d604-4504-9b04-8d872bb88c34)

![image](https://user-images.githubusercontent.com/111363234/205210376-eadcc339-4027-41f0-86ab-5496235c537f.png)

### 4. Enemy turns
Meanwhile, enemy-bot is attacking our field!

![image](https://github.com/user-attachments/assets/d1068a75-5edd-4686-b1be-fe636f8dcd45)

### 5. End of the Game
* Game stops the moment someone destroys all of the opponent ships. <br /> 
* When it happens you will be able to see where the enemy ships were located.

![image](https://github.com/user-attachments/assets/7b907d50-1b71-42fa-ad0f-cc7727382491)

# Enemy computer attack algorithm 
1. ![image](https://github.com/user-attachments/assets/f1628e26-42fd-4643-8133-54ed4828925f) ->
![image](https://github.com/user-attachments/assets/c5bf72c4-379f-42f3-8a61-30afe83ea920)
<br/> <br />
2. After finding the first piece next move is going to be random around the current piece (1 to 4 in this case) until it hits next time. <br/>
![image](https://github.com/user-attachments/assets/0042e17a-cb78-4c13-aa8e-792cb8239360) <br />
↓
<br /> ![image](https://github.com/user-attachments/assets/b3f03a89-0550-4e68-98cb-96783be61be3)
<br /> <br />
3. When it gets to the edge it checks if the edge's Y-axis coordinate is the same the original "hit" piece to determine whether the ship is horizontal.
<br /> <br />
![image](https://github.com/user-attachments/assets/6737f67f-928a-4445-ba7d-a3fe1d896752)
<br /> <br />
4. Now it goes back to the original ship piece shot and moves the opposite direction (up/down or left/right depending on the condition if the ship is horizontal, which it determined last step).
<br /> <br /> ![image](https://github.com/user-attachments/assets/ae09fe2e-be8a-497d-a09e-0c2a957b908e)





 <br />
* Enemy algorithm works the same with any ship size and it also understands if the cell was already shot and attacks the opposite way. <br />
* This algorithm is the most effective way to play "Sea Battle" and usually people use the same strategy. <br /> <br />
