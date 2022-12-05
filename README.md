# "Sea Battle" Windows Forms
Classic "Sea Battle" game on Windows Forms with Enemy bot

> *Place ships panel* => *choose ship size* => *mouse cursor on the field*

![image](https://user-images.githubusercontent.com/111363234/205208661-8c191fac-0f5f-4828-b15f-07611fa2db8e.png)

> *mouse left button*

![image](https://user-images.githubusercontent.com/111363234/205208823-f3327c16-c121-4d5b-8182-6a796878feaf.png)

> Ships horizontality can be changed <br /> <br />
![image](https://user-images.githubusercontent.com/111363234/205208967-d10b8f29-e66f-4c9a-8147-33f61f01f185.png) <br />
![image](https://user-images.githubusercontent.com/111363234/205208984-6e86da1c-7458-46c1-91e2-e99007d69d18.png) <br />

> Ships can be arranged randomly <br /> <br />
![image](https://user-images.githubusercontent.com/111363234/205209269-c927bd74-653e-4f27-93c8-a8092d3621d5.png)

After you place all the ships choose enemy settings and press "Start Game"

![image](https://user-images.githubusercontent.com/111363234/205209496-fa32a483-32c4-4cb7-8076-79a4ef576701.png)

Press on the enemy field button. You and enemy go one by one. <br />
If the text of the button is dot - this cell is clear <br />
If cross - you hit the enemy ship

![image](https://user-images.githubusercontent.com/111363234/205210176-bdb95f8f-42e8-4a2d-93dd-af9b8d8bfff8.png)

Also status helps with that

![image](https://user-images.githubusercontent.com/111363234/205210209-139caa90-2075-4448-ad82-0b44f22a760d.png)

As you hit an enemy ship - you continue your moves until you miss.

As the enemy ship is dead - all the cells around gets covered

![image](https://user-images.githubusercontent.com/111363234/205210360-96f9751d-58a8-4a10-b747-39ba5ceb3ed9.png)

![image](https://user-images.githubusercontent.com/111363234/205210376-eadcc339-4027-41f0-86ab-5496235c537f.png)

Meanwhile, enemy-bot is attacking our field

![image](https://user-images.githubusercontent.com/111363234/205210557-3b03fdf9-acb7-4d98-900c-45030238bc0b.png)

Game stops as someone destroys all the enemy ships and you can see where they were located

![image](https://user-images.githubusercontent.com/111363234/205210651-2c50fc15-1bff-4c64-b8bf-a57bfaf3fc66.png)




# Enemy attack algorithm

![EnemyAttackTransparentDarkShadow](https://user-images.githubusercontent.com/111363234/205208423-ea1800c2-4437-42e5-936c-216f3fc9a110.png) <br />
Works the same with any ship (also understands if the cell was already shot and goes the opposite way) <br />
This algorithm is the most effective way to play "Sea Battle" (usually people play same) <br />
