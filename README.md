# GE2_Assignment - DOTS Space battle 

Name: Tadhg O'Rourke

Student Number: C17403574

Class Group: DT228

## Background

This project will utilise Unitys Data Oriented Technology Stack (DOTS) to create simulated space battles. This project will use several steering behaviours to control the movement of space ships in the battle. This project will use several additional DOTS technologies to improve performace, including DOTS Physics to simulate some physics on the spaceship entities.

## Unity Packages Used
- Entities
- Jobs
- Burst
- DOTS Physics
- DOTS Editor
- Mathematics
- Hybrid Renderer

## Story Board

The universe is not a peaceful as you may believe. The allied force, a conglomeration of planets and groups looking to defend justice and protect the universe from destruction. The enemy force, a society of only the strong who seek to control all people for their own benifit and amusement. These forces have been warring for thousands of years, leading to the desutruction of many thousands of planets and the loss of millions of lives. The battles this simulation depicts are just some of the smaller skirmishes that these two great forces have fought.   
 
 --- 
 
This project will have several different spaceship models on each force. These ships have been split into different spaceship classes based on their size. the following legend shows their class relative to their size.

![Legend](https://i.imgur.com/PSn9xuc.png)

---

These battles are occur in empty intergalactic space, showing anthing can happen any where in the universe. Hundreds of ships on both sides will appear and being to battle.  

![Intro](https://i.imgur.com/HGqEm1a.png)

---

As the battle occurs ships will wander looking for ships to battle. Once found a ship will be able lock onto a target they will pursue them until one of them is destroyed. Within a certain distance the ship will being firing upon their target. When that target is destroyed they will find another spaceship to attack. If a ship is targeted they will attempt to flee for their lives, so they may live on to continue the fight. The battle ends when one force ships number less than 40. After the defeat of one of the forces the battle will end and another will begin, showing the never ending struggle between these two sides. 

![Battle](https://i.imgur.com/jMOcmJv.png)

## Graphical Techniques
Added trail renderers to each ship to act as engine exhaust
![image](https://user-images.githubusercontent.com/44297424/118962962-daddd400-b95d-11eb-8332-b8eeeff6d405.png)

## Included Steering Behaviours
- Arrive
- Constrain
- Flee
- Pursue
- Seek
- Wander (Jitter)

## Used Steering Behaviour
- Constrain
- Pursue
- Flee
- Wander (Jitter)

# Statemachine
This project attempts to use a DOTS version of a State Machine. A DOTS State Machine acts differently to a standard Finite State Machine as DOTS best practices is to have seperate systems/jobs for each seperate operation on an entity, the transitions between active states should handled by different systems, where the conditions for a state to be active are queried. As such, this project uses several seperate ECS systems to enable and disable different states. To accompany this, another system is used to help enable and disable specific steering behaviours depending on the state a ship could be in, i.e., if a ship is in a fleeing state, the wandering and fleeing steering behaviours become active. The following systems perform the transitions between states:
- **AttackStateSystem** - Check the distance to a target ship, if within a threshold distance start attacking (Shooting)
- **FleeStateSystem** - If the ship is being chased by a target, begin fleeing from pursuer
- **PursueStateSystem** - If the ship has found a target, begin pursing that target
- **SearchStateSystem** - if the ship does not have a target, search for a target

## Classes
Most classes within this project are origional with exception to some steering related systems. 

The following systems have taken from referenced projects and adjusted for the needs of this project:
- **BoidSystem/BoidJob** - modified from ECS2020
- **ArriveJob** - modified from GE2-2020-2021
- **ConstrainJob** - modified from ECS2020
- **FleeJob** - modified from ECS2020
- **PursueJob** - modified from GE2-2020-2021
- **SeekJob** - modified from GE2-2020-2021
- **WanderJob** - modified from ECS2020


## Art Assets
- Space Ship assets - https://assetstore.unity.com/packages/3d/vehicles/space/spaceship-voxel-87921
- Space Skybox assets - https://assetstore.unity.com/packages/2d/textures-materials/deep-space-skybox-pack-11056

## Referenced Projects
- https://github.com/skooter500/GE2-2020-2021
- https://github.com/skooter500/ECS2020
- https://github.com/Unity-Technologies/EntityComponentSystemSamples

## Camera Options
The project will have the ability to switch between several different camera viewpoints:
* panning/birds eye view
* follow random ship

## Controls
- Spacebar to cycle between camera options
- Mouse0 (Left-click) to cycle between camera targets

## Video
[![DOTS Space Battle](http://i3.ytimg.com/vi/Zfc9NKxJ180/maxresdefault.jpg)](https://youtu.be/Zfc9NKxJ180)
