package com.wawrzyniak.kukaComm.Controller;

import com.wawrzyniak.kukaComm.Exceptions.RobotNotConfiguredException;
import com.wawrzyniak.kukaComm.Exceptions.WrongIpException;
import com.wawrzyniak.kukaComm.Model.ModelReading.ConfiguredRobotDTO;
import com.wawrzyniak.kukaComm.Model.ModelReading.RobotData;
import com.wawrzyniak.kukaComm.Service.RobotData.ConfiguredRobotService;
import com.wawrzyniak.kukaComm.Service.RobotData.RobotModelService;
import com.wawrzyniak.kukaComm.Service.RobotData.RobotStickerService;
import lombok.RequiredArgsConstructor;
import org.springframework.web.bind.annotation.*;

import java.io.IOException;
import java.util.List;
import java.util.Map;

@RestController
@RequestMapping("/kuka-variables/")
@RequiredArgsConstructor
public class StaticDataController {

    private final RobotModelService robotModel;
    private final RobotStickerService robotSticker;
    private final ConfiguredRobotService robotService;


    @GetMapping("configured")
    public Map<String, Map<String, RobotData>> getAllRobots() throws IOException {
        return robotModel.getAvailableRobots();
    }

    @GetMapping("stickers")
    public Map<String, byte[]> getAllStickers() throws IOException {
        return robotSticker.getAllStickers();
    }

    @GetMapping("robots")
    public List<ConfiguredRobotDTO> getALLRobotsWithStickers() {

        return robotService.getAllConfiguredRobots();
    }
    @GetMapping("robot/{ip}")
    public ConfiguredRobotDTO getRobotByIp(@PathVariable String ip) throws RobotNotConfiguredException {
        return robotService.getRobotByIp(ip);
    }

    @PostMapping("add")
    public ConfiguredRobotDTO addRobot(@RequestBody ConfiguredRobotDTO robotDTO) throws WrongIpException {
        return robotService.save(robotDTO);
    }
    @PostMapping("update")
    public ConfiguredRobotDTO updateRobot(@RequestBody ConfiguredRobotDTO robotDTO) throws RobotNotConfiguredException {
        return robotService.updateByIp(robotDTO);
    }

    @DeleteMapping("delete/{ip}")
    public void deleteRobot(@PathVariable String ip) {
        robotService.deleteByIp(ip);
    }
}
