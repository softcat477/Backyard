# Backyard
> (╯°□°）╯ 🌈 ┻━┻
>
https://github.com/softcat477/Backyard/assets/25975988/2db9c1a6-2946-4038-8232-1dc5ec5aad7f

A scene to let the player throw stuff by launching a projectile, predicting its trajectory to see its landing position, and throwing the projectile!

## What is going on in this scene
- Predict A trajectory: One can click and hold to launch a projectile, drag to move the transparent cube indicating the predicted landing position, and release to fire the projectile!
  - Keywords: `Rigidbody`, `Line Renderer`, `InputSystem`, high school physics, lots of 3d math
- Move the character: Move the character movement with `WASD` and rotate the character with the mouse.
  - Keywords: `InputSystem`, `CharacterController`
- Multiversity in projectiles: there are projectiles that are just balls; there are projectiles that stick on whatever surfaces they're touching; there're projectiles that exchange their colour with surfaces they're touching. Hold and scroll to use different projectiles!
  - Keywords: `Renderer`, `RigidBody`, `InputSystem`
- Projectile Sound Effect: Projectiles have their own sound. Some sound muffled, and some sound grating.
  - Keywords: `fmod`
