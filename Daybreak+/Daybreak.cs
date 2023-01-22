using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlockEntities.Implementations;
using ModLoaderInterfaces;
using Pipliz;
using static BlockEntities.Implementations.BannerTracker;

namespace Daybreak_
{
    public class Daybreak : IOnUpdate
    {
        public static float lastTime = 0;
        public void OnUpdate()
        {
            float currentTime = TimeCycle.TimeOfDayHours;

            // check for daybreak/nightfall
            if (System.Math.Abs(lastTime) > 0.05)
            {
                if (currentTime > TimeCycle.SunRise && lastTime <= TimeCycle.SunRise)
                {
                    //Log.Write("Playing Daybreak sound at all active banners.");

                    for (int i = 0; i < Players.ConnectedPlayers.Count; i++)
                    {
                        PlaySoundAtAllActiveBanners(Players.ConnectedPlayers[i], true);
                    }
                }
                if (currentTime > TimeCycle.SunSet && lastTime <= TimeCycle.SunSet)
                {
                    //Log.Write("Playing Nightfall sound at all active banners.");

                    for (int i = 0; i < Players.ConnectedPlayers.Count; i++)
                    {
                        PlaySoundAtAllActiveBanners(Players.ConnectedPlayers[i], false);
                    }
                }
            }

            lastTime = currentTime;
        }

        public void PlaySoundAtAllActiveBanners(Players.Player player, bool isDaybreakSound)
        {
            for (int i = 0; i < player.ActiveColonyGroup.Colonies.Count; i++)
            {
                for (int i2 = 0; i2 < player.ActiveColonyGroup.Colonies[i].Banners.Count; i2++)
                {
                    if (isDaybreakSound)
                        PlaySoundAtBanner(player.ActiveColonyGroup.Colonies[i].Banners[i2], "daybreak");
                    else
                        PlaySoundAtBanner(player.ActiveColonyGroup.Colonies[i].Banners[i2], "nightfall");
                }
            }
        }

        public void PlaySoundAtBanner(BannerTracker.Banner banner, string soundname)
        {
            UnityEngine.Vector3 bannerpos = new UnityEngine.Vector3(banner.Position.x, banner.Position.y, banner.Position.z);
            AudioManager.SendAudio(bannerpos, soundname);
        }
    }
}
