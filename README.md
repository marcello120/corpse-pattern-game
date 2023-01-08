# corpse-pattern-game
it is what it is.


# Enemy guide

To Make Enemy:
 
Create GameObject
Create Script for enemy that extends Enemy class
Start must call base.Init();
Add script to gameObject
Add the following components to GameObject:
-animator(new)
-Collider (adjust edges)
-RigidBody2D(copy from Slime)
-SpriteRender

In the script component
-Add Corpse prefab
-set Health, Attack Power, Movement Speed,
-set Enemyname

Set up animations for: Idle, Hit, Run,Death
-Death must call Remove() on last frame.

Set up animator variables:
-Death(trigger)
-Hit(trigger)
-IsMoving(bool)

Connect animations in animator

Update should handle movement,
-for default detection zone movement add a detection zone and use moveToPlayerWithDetectionZone(detectionZoneController)

OnCollisionEnter2D should handle damage to player
-for default use damagePlayer(collision.collider);

if required provide overrides for Remove, getHit, Death and Init

