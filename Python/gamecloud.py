import urllib2
import json
import pprint

class GameCloud:
    def __init__(self):
        self.authKey = "asd"
        self.playerId = "atestplayerInGame"
        self.server_url = "http://54.220.223.184:8888"

    def do_request(self, data):
        data = json.dumps(data)
        
        req = urllib2.urlopen(self.server_url, data)
        ret = req.read()
        req.close()
        
        return json.loads(ret)
    #
    
    def giveAchievement(self, achi):
        data = dict(
            callType = "gameDataSave",
            authkey = self.authKey,
            hash = achi_mapping_give[achi],
            playerId = self.playerId
        )
        
        return self.do_request(data)
    #
    
    def hasAchievement(self, achi):
        data = dict(
            callType = "ask",
            authkey = self.authKey,
            hash = achi_mapping_get[achi],
            playerId = self.playerId
        )
        
        return self.do_request(data)
#

achi_mapping_give = dict(
    JustaSuckyThing = "kzogp8hm9j4f5hfr",
)
achi_mapping_get = dict(
    JustaSuckyThing = "e1iqm81m1qg833di",
)


if __name__ == "__main__":
    cloud = GameCloud()
    
    pprint.pprint(cloud.giveAchievement("JustaSuckyThing"))
    pprint.pprint(cloud.hasAchievement("JustaSuckyThing"))
