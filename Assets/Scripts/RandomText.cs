using UnityEngine;
using System.Collections;

public class RandomText : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		string text;
		switch(Random.Range(0,43)) {
			case 0:  text="Welcome back!"; break;
			case 1:  text="It's not always safe\nto look at these signs..."; break;
			case 2:  text= "Please, vote for this game\nif you like it"; break;
			case 3:  text= "It's really hard to come\nup with 100 sentences..."; break;
			case 4:  text="Wololoooo!!"; break;
			case 5:  text="Gotta catch'em all!"; break;
			case 6:  text="Time is running out..."; break;
			case 7:  text="This game's OST is amazing!!!"; break;
			case 8:  text="Warning: Pixls on rails"; break;
			case 9:  text= "Play this with a\nfriend for extra mojo"; break;
			case 10: text= "Congrats!!\nYou are the pixl 1.000.000!!!"; break;
			case 11: text= "This game was made in 30h"; break;
			case 12: text= "Thank god for the\ntheme not being potato"; break;
			case 13: text="All your votes are belong to us!"; break;
			case 14: text="How far can you get..."; break;
			case 15: text="Losing is fun!"; break;
			case 16: text="This is where dead pixls go"; break;
			case 17: text="Follow the nyan!!"; break;
			case 18: text="You shall not pass!"; break;
			case 19: text= "48 hours is not\na lot of time..."; break;
			case 20: text="SuperMeatBoy rests here"; break;
			case 21: text= "All the blocks\nprovided by Minecraft"; break;
			case 22: text= "Follow the\nbrick road!"; break;
			case 23: text= "You should\nplay Monaco on Steam"; break;
			case 24: text= "100% pseudo-isometric-\nphysic-platformer"; break;
			case 25: text= "Share you opinion about Pixl\nat www.origo.by"; break;
			case 26: text= "The hardest part of LD\nis to finish!"; break;
			case 27: text="You are doing great!"; break;
			case 28: text= "Minimalist was quite\nambiguous for my taste..."; break;
			case 29: text= "Use the distance display\nas depth reference"; break;
			case 30: text= "No pixls where harmed\nwhile making this game"; break;
			case 31: text= "Use Jump+ChangeTrack for\na (kind of) double jump"; break;
			case 32: text= "I wonder if anyone\nwill read this?"; break;
			case 33: text= "I see a lot of cubes happening\nin this LD..."; break;
			case 34: text= "This one is actually a grave,\na very tall grave"; break;
			case 35: text= "It's dangerous to play alone!\nTake a friend!"; break;
			case 36: text= "These two are actually\n2/3rds of a full pixl"; break;
			case 37: text= "This is my 1st LD!\nHope you like it :)"; break;
			case 38: text= "Thanks for playing!\nKeep going!"; break;
			case 39: text= "At the bottom there is...\nem............ Lava!!!!"; break;
			case 40: text= "I'm running out of ideas\nfor these texts..."; break;
			case 41: text= "I love to end sentences\nwith three dots..."; break;
			default: text="Keep going!"; break;
		}
		GetComponent<TextMesh>().text=text;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
