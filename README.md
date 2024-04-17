![Total Downloads](https://img.shields.io/github/downloads/THQuery/BoombasticComedyCountdown-Exiled/total)

# BoombasticComedyCountdown - Exiled - SCP: SL
Added a countdown after the player carves a Grenade or Grenade Flash. If the player doesn't throw or cancel it in the allotted time, the Grenade will start immediately. Boooommmmbababab

This plugin is only compatible with [Exiled Framework](https://github.com/Exiled-Team/EXILED) --- 8.8.1 +

## Configuration
```yaml
boombastic_comedy_countdown:
  is_enabled: true
  debug: true
  # Enable or disable the timer system for the Grenade.
  enable_grenade_timer: true
  # Enable or disable the timer system for the Flash.
  enable_flash_timer: true
  # The duration of the timer for the Grenade in milliseconds.
  grenade_timer_duration: 6
  # The duration of the timer for the Flash in milliseconds.
  flash_timer_duration: 6
  # The text to display when the grenade will explode.
  grenade_explode_text: '<color=yellow>Grenade will explode in</color> <color=red>{0}</color> <color=yellow>seconds</color>'
  # The text to display when the flashbang will explode.
  flash_explode_text: '<color=yellow>Flash will explode in</color> <color=red>{0}</color> <color=yellow>seconds</color>'
  # The time in seconds before the grenade explodes.
  grenade_fuse_time: 0.100000001
  # The time in seconds the grenade will concuss players.
  grenade_concuss_duration: 5
  # The time in seconds the grenade will burn players.
  grenade_burn_duration: 5
  # The time in seconds before the flashbang explodes.
  flash_fuse_time: 0.100000001
```
