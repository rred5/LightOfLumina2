INCLUDE globals.ink


#speaker:Elian #portrait:elian_neutral #layout:left #audio:animal_crossing_mid

- Hey! I wanted to offer you guys and any of your friends the opportunity 
to work in my company.

#speaker:Worker #portrait:worker_neutral #layout:right #audio:animal_crossing_low
-Sure, sounds good, me and my friend here can start working immediately.
- Where should I go?

#speaker:Elian #portrait:elian_neutral #layout:left #audio:animal_crossing_mid
- My business building is just north of here. 
- Thanks for the help!
~ businessFunction("hireFirst")
//Make this hire 2 workers
~ storyEvent("enableWorkerHint")
//Make this enable the new worker position and disable itself, or just disable the visual and the exclamation if it makes errors


-> END