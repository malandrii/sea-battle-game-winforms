# "Sea Battle" on Windows Forms
Classic "Sea Battle" game on Windows Forms with Enemy bot

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
Press on the enemy field button. You and enemy go one by one. <br />
If the text of the button is a dot - this cell is clear. <br />
If the text of the button is a cross - you hit the enemy ship!

![image](https://github.com/user-attachments/assets/d1a34138-4dab-4971-adec-48d148c0c4b3)

Status also helps you monitoring current move action

![image](https://user-images.githubusercontent.com/111363234/205210209-139caa90-2075-4448-ad82-0b44f22a760d.png)

When you hit an enemy ship you will be able to continue your moves until you miss. <br />
When the enemy ship becomes dead - all the cells around it get covered in dots.

![image](https://github.com/user-attachments/assets/e0f3735b-d604-4504-9b04-8d872bb88c34)

![image](https://user-images.githubusercontent.com/111363234/205210376-eadcc339-4027-41f0-86ab-5496235c537f.png)

### 4. Enemy turns
Meanwhile, enemy-bot is attacking our field!

![image](https://github.com/user-attachments/assets/d1068a75-5edd-4686-b1be-fe636f8dcd45)

### 5. End of the Game
Game stops the moment someone destroys all the opponent ships. <br /> 
When it happens you will be able to see where enemy ships were located.

![image](https://github.com/user-attachments/assets/7b907d50-1b71-42fa-ad0f-cc7727382491)

# Enemy attack algorithm

![EnemyAttackTransparentDarkShadow](https://user-images.githubusercontent.com/111363234/205208423-ea1800c2-4437-42e5-936c-216f3fc9a110.png) <br />
Works the same with any ship size (also understands if the cell was already shot and goes the opposite way) <br />
This algorithm is the most effective way to play "Sea Battle" (usually people play the same way) <br />
