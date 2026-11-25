# How to set up Discord integration in Unity project

The Discord Game SDK is already included in JUtils, so there's nothing to download. It works on Windows only.

---

## 1. Create the Discord application

1. Go to https://discord.com/developers/applications and click **New Application**.
2. Give it your game's name. This is the name Discord shows after "Playing".
3. On the **General Information** page, copy the **Application ID**.

## 2. Add it to the scene

1. In your first scene, create an empty GameObject and add the `JDiscordManager` component to it.
2. Paste the Application ID into the **Application Id** field.
