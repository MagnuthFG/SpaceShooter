# Space Shooter
## Project Summary
Created an arcade-like top down space shooter with minimalistic 2D visuals. <br />
I used both Monobehaviour and ECS in this project. Mono used 2D Physics while ECS used AABB calculations.

## [Minimum Optimization](https://github.com/MagnuthFG/SpaceShooter/releases/tag/Minimal-Optimization)
I first created a standard Monobehaviour version, using pooling for projectiles and enemies to lower the amount of work the garbage
collection had to do and prevent potential lag spikes, instantiating and deleting objects.

The most costly calculation in Mono version at 5000 enemies was Physics2D.SyncTransformChanges and Physics2D.Step at around 25% and 23%.
Enabling Multithreaded 2D physics lowered them to around 20% and 6% and increased fps from 30-40 23ms to 60~80 13ms.

The second most costly calculation was my EnemyMove script at 20% which was added to each asteroid and updated them individually. 
Having one move script that looped over all the asteroids in a list would have been a more performant and saved on update events 
as well as lowered the cost of moving between C++ and C#.

I also toggled game objects when moving them in and out of the object pools, which could have been optimized as well to prevent unnecesary
looping over components and toggling them and firing off on enabled and on disabled event calls (and other stuff on units own components).

## [First Optimization](https://github.com/MagnuthFG/SpaceShooter/releases/tag/First-Optimization)
I decided to replaced the player, enemies and projectiles into ECS entities, because I wanted to messure the difference between them. 
At first I didn't see any benefit, which was most likely because I treated it like Mono, using queries and forloops rather then jobs and
entities.foreach. Only when I reached high enemy counts, is when I started seeing the benefits of using a data oriented approach.

I used no physics in this version, and instead does AABB checks by reusing the world render bounds component.
This could have been optimized further by using a radius check rather then boundary check to reduce on mathematical calculations, 
but in the end felt this worked just fine.

## [Second Optimization](https://github.com/MagnuthFG/SpaceShooter/releases/tag/Second-Optimization)
The most costly calculation was my EnemyMove and EnemyDamage systems at around 37% and 20% 10ms. Changing my code to use entity 
foreach and parallel multithreading to spread the work on multiple cores lowered both to about 1% and increased fps from 90-100 10ms to 240-260 5ms.

I didn't have time to optimize the rest of the scripts, due to pressing portfolio work, sorry about that.
How ever at the low enemy count you'll encounter, the performance is so high, they're not going to be a problem.
If I would have optimized them, I would have used the same approach as I did with the movement and damage systems.

## Performance Tests
GPU ms followed CPU ms quite close, although a little bit lower

500 enemies &emsp; &emsp; 300 - 350fps  3ms (Mono) <br />
500 enemies &emsp; &emsp; 240 - 250fps  4ms (ECS)

1000 enemies &emsp; &emsp; 200 - 250fps  4ms (Mono)  <br />
1000 enemies &emsp; &emsp; 200 - 220fps  5ms (ECS)

2000 enemies &emsp; &emsp; 180 - 200fps  5ms (Mono) <br />
2000 enemies &emsp; &emsp; 160 - 170fps  6ms (ECS)

3000 enemies &emsp; &emsp; 100 - 140fps  8ms (Mono) <br />
3000 enemies &emsp; &emsp; 130 - 140fps  7ms (ECS)

4000 enemies &emsp; &emsp; 80 -  90fps 13ms (Mono) <br />
4000 enemies &emsp; &emsp; 112 - 117fps  8ms (ECS)

5000 enemies &emsp; &emsp; 30 -  40fps 23ms (Mono) <br />
5000 enemies &emsp; &emsp; 90 - 100fps 10ms (ECS)

5000 enemies &emsp; &emsp; 60 -  80fps 13ms (Mono) multithreaded 2D physics <br />
5000 enemies &emsp; &emsp; 240 - 260fps  5ms (ECS) multithreading + burst

