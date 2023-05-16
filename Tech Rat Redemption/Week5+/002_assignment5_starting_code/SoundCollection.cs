using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine;

namespace GXPEngine
{
    public class SoundCollection
    {
        private ArrayList soundFiles = new ArrayList();
        SoundChannel channel;
        bool noStop;
        public SoundCollection()
        {
            AddSound("Orb_goes_in_basket.wav"); //0
            AddSound("Launchpad_Jump.wav"); //1
            AddSound("Button_hit.wav"); //2
            AddSound("Gate.wav"); //3
            AddSound("Gunshot 1.wav"); //4
            AddSound("Orb_spawns.wav"); //5
            AddSound("Orb_spawns_2.wav"); //6
            AddSound("Orb_spawns_3.wav"); //7
            AddSound("Orb_hits_wall_1.wav"); //8
            AddSound("Orb_hits_wall_2.wav"); //9
            AddSound("Orb_hits_wall_3.wav"); //10
            AddSound("Orb_hits_wall_4.wav"); //11
            AddSound("Orb_hits_wall_5.wav"); //12
            AddSound("GameplayMusicLevelOne.wav"); //13
            AddSound("GameplayMusicLevelTwo.wav"); //14
            AddSound("Gunshot 2.wav"); //15
            AddSound("Gunshot 3.wav"); //16
            AddSound("Gunshot 4.wav"); //17
            AddSound("Gunshot 5.wav"); //18
            AddSound("Orb_hits_wall_1.wav"); //19
            AddSound("Orb_hits_wall_2.wav"); //20
            AddSound("Orb_hits_wall_3.wav"); //21
            AddSound("Orb_hits_wall_4.wav"); //22
            AddSound("Orb_hits_wall_5.wav"); //23
            AddSound("Bullet hits orb.wav"); //24
            AddSound("Button_UI.wav"); //25
        }
        public void PlaySound(int index, bool loop = false, bool stream = false)
        {
            Sound soundEffect = new Sound(soundFiles[index].ToString(), loop, stream);
            soundEffect.Play();
        }

        public void AddSound(string soundFile)
        {
            soundFiles.Add(soundFile);
        }

        public void PlayMusic(int index, bool pNoStop = false, bool loop = true, bool stream = false)
        {
            if(channel != null)
            {
                channel.Stop();
            }
            noStop = pNoStop;
            Sound music = new Sound(soundFiles[index].ToString(),loop, stream);
            channel = music.Play();
        }

        public void StopMusic()
        {
            if (channel != null)
            {
                Console.WriteLine(noStop);
                if(!noStop)
                {
                    channel.Stop();
                }
            }
        }

        public void ShowSoundArray()
        {
            int index = 0;
            foreach (string soundFile in soundFiles)
            {
                Console.WriteLine("[" + index + "] " + soundFile);
                index++;
            }
        }
    }
}