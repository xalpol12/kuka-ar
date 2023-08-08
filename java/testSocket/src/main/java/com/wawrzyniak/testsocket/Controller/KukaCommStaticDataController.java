package com.wawrzyniak.testsocket.Controller;


import com.wawrzyniak.testsocket.Model.Records.RobotData;
import com.wawrzyniak.testsocket.Service.ImageService;
import com.wawrzyniak.testsocket.Service.KukaMockService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;

import java.io.IOException;
import java.util.Map;

@RestController
@RequestMapping("/kuka-variables/")
public class KukaCommStaticDataController {

    private final KukaMockService kukaService;
    private final ImageService imageService;

    @Autowired
    KukaCommStaticDataController(KukaMockService service, ImageService imageService){
        kukaService = service;
        this.imageService = imageService;
    }

    @GetMapping("configured")
    public Map<String, Map<String, RobotData>> getAllRobots(){
        return kukaService.getAvailableRobots();
    }

    @GetMapping("stickers")
    public Map<String, byte[]> getAllStickers() throws IOException {
        return imageService.getAllStickers();
    }

}
