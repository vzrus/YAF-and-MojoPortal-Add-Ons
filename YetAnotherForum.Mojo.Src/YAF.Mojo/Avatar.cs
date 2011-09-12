using System;
using YAF.Classes.Data;
using YAF.Core;
using YAF.Types.EventProxies;
using YAF.Types.Interfaces;

namespace YAF.Mojo
{
    public static class Avatar
    {
        /// <summary>
        /// Save avatar path.
        /// </summary>
        /// <param name="avatarPath">
        /// The file info avatar.
        /// </param>
        /// <param name="yafUserId">
        /// The yaf user id.
        /// </param>
        public static void SaveAvatar(string avatarPath, int yafUserId)
        {
            // update
           // 
            LegacyDb.user_saveavatar(
                yafUserId,
                String.Format(
            "{0}{1}","", avatarPath),
                null,
                null);

            // clear the cache for this user...
            YafContext.Current.Get<IRaiseEvent>().Raise(new UpdateUserEvent(yafUserId));
        }
    }
}
