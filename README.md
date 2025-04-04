## Warning
no auth included!!!
do not expose to the internet.

# ez-uptime
super minimalistic uptime monitor with yaml config

This is a small project written in a couple of hours. 
I just needed something simple for my homelab.

## Example config
```yaml
Computers:
- label: PC-2
  type: Ping
  address: 192.168.0.2
Websites:
- label: Google
  type: HttpGet
  address: https://google.com
```
