package com.wawrzyniak.kukaComm.Controller;

import com.wawrzyniak.kukaComm.Exceptions.RobotNotConfiguredException;
import com.wawrzyniak.kukaComm.Model.ModelReading.ConfiguredRobotDTO;
import com.wawrzyniak.kukaComm.Model.ModelReading.RobotData;
import com.wawrzyniak.kukaComm.Service.RobotData.ConfiguredRobotService;
import com.wawrzyniak.kukaComm.Service.RobotData.RobotModelService;
import com.wawrzyniak.kukaComm.Service.RobotData.RobotStickerService;
<<<<<<< refs/remotes/origin/main
<<<<<<< refs/remotes/origin/main
import lombok.RequiredArgsConstructor;
=======
import org.springframework.beans.factory.annotation.Autowired;
>>>>>>> add testSocket and kukaComm
=======
import lombok.RequiredArgsConstructor;
>>>>>>> add swagger docs, fix some whitespace issues
import org.springframework.web.bind.annotation.*;

import java.io.IOException;
import java.util.List;
import java.util.Map;

@RestController
@RequestMapping("/kuka-variables/")
<<<<<<< refs/remotes/origin/main
<<<<<<< refs/remotes/origin/main
@RequiredArgsConstructor
=======
>>>>>>> add testSocket and kukaComm
=======
@RequiredArgsConstructor
>>>>>>> add swagger docs, fix some whitespace issues
public class StaticDataController {

    private final RobotModelService robotModel;
    private final RobotStickerService robotSticker;
    private final ConfiguredRobotService robotService;

<<<<<<< refs/remotes/origin/main
<<<<<<< refs/remotes/origin/main
=======
    @Autowired
    StaticDataController(RobotModelService robotService, RobotStickerService robotSticker, ConfiguredRobotService robotService1){
        this.robotModel = robotService;
        this.robotSticker = robotSticker;
        this.robotService = robotService1;
    }

>>>>>>> add testSocket and kukaComm
=======
>>>>>>> add swagger docs, fix some whitespace issues
    @GetMapping("configured")
    public Map<String, Map<String, RobotData>> getAllRobots() throws IOException {
        return robotModel.getAvailableRobots();
    }

    @GetMapping("stickers")
    public Map<String, byte[]> getAllStickers() throws IOException {
        return robotSticker.getAllStickers();
    }

    @GetMapping("robots")
<<<<<<< refs/remotes/origin/main
<<<<<<< refs/remotes/origin/main
    public List<ConfiguredRobotDTO> getALLRobotsWithStickers() {
=======
    public List<ConfiguredRobotDTO> getALLRobotsWithStickers(){
>>>>>>> add testSocket and kukaComm
=======
    public List<ConfiguredRobotDTO> getALLRobotsWithStickers() {
>>>>>>> add swagger docs, fix some whitespace issues
        return robotService.getAllConfiguredRobots();
    }
    @GetMapping("robot/{ip}")
    public ConfiguredRobotDTO getRobotByIp(@PathVariable String ip) throws RobotNotConfiguredException {
        return robotService.getRobotByIp(ip);
    }

    @PostMapping("add")
<<<<<<< refs/remotes/origin/main
<<<<<<< refs/remotes/origin/main
    public ConfiguredRobotDTO addRobot(@RequestBody ConfiguredRobotDTO robotDTO) {
=======
    public ConfiguredRobotDTO addRobot(@RequestBody ConfiguredRobotDTO robotDTO){
>>>>>>> add testSocket and kukaComm
=======
    public ConfiguredRobotDTO addRobot(@RequestBody ConfiguredRobotDTO robotDTO) {
>>>>>>> add swagger docs, fix some whitespace issues
        return robotService.save(robotDTO);
    }
    @PostMapping("update/{ip}")
    public ConfiguredRobotDTO updateRobot(@RequestBody ConfiguredRobotDTO robotDTO) throws RobotNotConfiguredException {
        return robotService.updateByIp(robotDTO);
    }

    @DeleteMapping("delete/{ip}")
<<<<<<< refs/remotes/origin/main
<<<<<<< refs/remotes/origin/main
    public void deleteRobot(@PathVariable String ip) {
        robotService.deleteByIp(ip);
    }
=======
    public void deleteRobot(@PathVariable String ip){
        robotService.deleteByIp(ip);
    }



>>>>>>> add testSocket and kukaComm
=======
    public void deleteRobot(@PathVariable String ip) {
        robotService.deleteByIp(ip);
    }
>>>>>>> add swagger docs, fix some whitespace issues
}
