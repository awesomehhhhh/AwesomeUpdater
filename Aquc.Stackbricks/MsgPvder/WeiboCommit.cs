﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Aquc.Stackbricks.MsgPvder;

public class WeiboCommitMsgPvder : IStackbricksMsgPvder
{
    public string MsgPvderId => ID;
    public const string ID = "stbks.msgpvder.weibocmts";

    public async Task<StackbricksUpdateMessage> GetUpdateMessage(StackbricksManifest stackbricksManifest)
    {
        var url = $"https://weibo.com/ajax/statuses/buildComments?flow=1&is_reload=1&id={stackbricksManifest.MsgPvderData}&is_show_bulletin=2&is_mix=0&count=10&fetch_level=0";
        var response = JsonNode.Parse(await StackbricksProgram._httpClient.GetAsync(url).Result.EnsureSuccessStatusCode().Content.ReadAsStringAsync())!;
        var message = response["data"]!.AsArray()[0]!["text"]!.ToString().Split(";;");
        StackbricksProgram.logger.Debug($"{message[0]}: Successful received update message: ver={message[1]}, pkg_pvder={message[2]}");
        return new StackbricksUpdateMessage(
            stackbricksManifest,
            new Version(message[1]),
            message[2],
            message[3][..^1] //?
            );
    }
}