package com.wawrzyniak.kukaComm.Controller;

import com.wawrzyniak.kukaComm.Exceptions.RobotAlredyConfiguredException;
import com.wawrzyniak.kukaComm.Exceptions.RobotNotConfiguredException;
import com.wawrzyniak.kukaComm.Exceptions.WrongIpException;
import com.wawrzyniak.kukaComm.Model.ModelReading.ConfiguredRobotDTO;
import com.wawrzyniak.kukaComm.Model.ModelReading.RobotData;
import com.wawrzyniak.kukaComm.Service.RobotData.ConfiguredRobotService;
import com.wawrzyniak.kukaComm.Service.RobotData.RobotModelService;
import com.wawrzyniak.kukaComm.Service.RobotData.RobotStickerService;
import lombok.RequiredArgsConstructor;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.web.bind.annotation.*;

import java.io.IOException;
import java.util.List;
import java.util.Map;

@RestController
@RequestMapping("/kuka-variables/")
@RequiredArgsConstructor
public class StaticDataController {

    private static final Logger logger =
            LoggerFactory.getLogger(StaticDataController.class);

    private final RobotModelService robotModel;
    private final RobotStickerService robotSticker;
    private final ConfiguredRobotService robotService;


    @GetMapping("configured")
    public Map<String, Map<String, RobotData>> getAllRobots() throws IOException {
        logger.debug("Called: GET /configured");
        return robotModel.getAvailableRobots();
    }

    @GetMapping("stickers")
    public Map<String, byte[]> getAllStickers() throws IOException {
        logger.debug("Called: GET /stickers");
        return robotSticker.getAllStickers();
    }

    @GetMapping("robots")
    public List<ConfiguredRobotDTO> getALLRobotsWithStickers() {
        logger.debug("Called: GET /robots");
        return robotService.getAllConfiguredRobots();
    }
    @GetMapping("robot/{ip}")
    public ConfiguredRobotDTO getRobotByIp(@PathVariable String ip) throws RobotNotConfiguredException {
        logger.debug("Called: GET /robot/{}", ip);
        return robotService.getRobotByIp(ip);
    }

    @PostMapping("add")
    public ConfiguredRobotDTO addRobot(@RequestBody ConfiguredRobotDTO robotDTO) throws WrongIpException, RobotAlredyConfiguredException {
        logger.debug("Called: POST /add with request body: {}", robotDTO);
        return robotService.save(robotDTO);
    }
    @PostMapping("update")
    public ConfiguredRobotDTO updateRobot(@RequestBody ConfiguredRobotDTO robotDTO) throws RobotNotConfiguredException {
        logger.debug("Called: POST /update with request body: {}", robotDTO);
        return robotService.updateByIp(robotDTO);
    }

    @DeleteMapping("delete/{ip}")
    public void deleteRobot(@PathVariable String ip) {
        logger.debug("Called: DELETE /delete/{}", ip);
        robotService.deleteByIp(ip);
    }
}
