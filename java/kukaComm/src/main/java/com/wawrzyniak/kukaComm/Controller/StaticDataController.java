package com.wawrzyniak.kukaComm.Controller;

import com.wawrzyniak.kukaComm.Exceptions.RobotAlredyConfiguredException;
import com.wawrzyniak.kukaComm.Exceptions.RobotNotConfiguredException;
import com.wawrzyniak.kukaComm.Exceptions.WrongIpException;
import com.wawrzyniak.kukaComm.Model.ModelReading.ConfiguredRobotDTO;
import com.wawrzyniak.kukaComm.Model.ModelReading.RobotData;
import com.wawrzyniak.kukaComm.Service.RobotData.ConfiguredRobotService;
import com.wawrzyniak.kukaComm.Service.RobotData.RobotModelService;
import com.wawrzyniak.kukaComm.Service.RobotData.RobotStickerService;
import io.swagger.v3.oas.annotations.Operation;
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

    @Operation(
            summary = "Returns all available robots",
            description = "Available robots are defined in /RobotData folder",
            tags = {"Config"})
    @GetMapping("configured")
    public Map<String, Map<String, RobotData>> getAllRobots() throws IOException {
        logger.debug("Called: GET /configured");
        return robotModel.getAvailableRobots();
    }

    @Operation(
            summary = "Returns all stickers",
            description = "Stickers are located in /RobotStickers folder",
            tags = {"Stickers"})
    @GetMapping("stickers")
    public Map<String, byte[]> getAllStickers() throws IOException {
        logger.debug("Called: GET /stickers");
        return robotSticker.getAllStickers();
    }

    @Operation(
            summary = "Returns all saved robots details",
            description = "Returns a whole table content from database",
            tags = {"Robots"})
    @GetMapping("robots")
    public List<ConfiguredRobotDTO> getALLRobotsWithStickers() {
        logger.debug("Called: GET /robots");
        return robotService.getAllConfiguredRobots();
    }

    @Operation(
            summary = "Returns saved robot details",
            description = "Returns a single row from database",
            tags = {"Robots"})
    @GetMapping("robot/{ip}")
    public ConfiguredRobotDTO getRobotByIp(@PathVariable String ip) throws RobotNotConfiguredException {
        logger.debug("Called: GET /robot/{}", ip);
        return robotService.getRobotByIp(ip);
    }

    @Operation(
            summary = "Saves new robot to a database",
            description = "Saves new robot and returns DTO of the saved robot",
            tags = {"Robots"})
    @PostMapping("add")
    public ConfiguredRobotDTO addRobot(@RequestBody ConfiguredRobotDTO robotDTO) throws WrongIpException, RobotAlredyConfiguredException {
        logger.debug("Called: POST /add with request body: {}", robotDTO);
        return robotService.save(robotDTO);
    }

    @Operation(
            summary = "Updates already saved robot details",
            description = "Updates robot details and returns DTO of the updated robot",
            tags = {"Robots"})
    @PutMapping("update")
    public ConfiguredRobotDTO updateRobot(@RequestBody ConfiguredRobotDTO robotDTO) throws RobotNotConfiguredException {
        logger.debug("Called: PUT /update with request body: {}", robotDTO);
        return robotService.updateByIp(robotDTO);
    }

    @Operation(
            summary = "Deletes robot from database",
            description = "Deletes saved robot from database by ip",
            tags = {"Robots"})
    @DeleteMapping("delete/{ip}")
    public void deleteRobot(@PathVariable String ip) {
        logger.debug("Called: DELETE /delete/{}", ip);
        robotService.deleteByIp(ip);
    }
}
