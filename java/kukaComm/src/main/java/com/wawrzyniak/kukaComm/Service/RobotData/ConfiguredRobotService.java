package com.wawrzyniak.kukaComm.Service.RobotData;

import com.wawrzyniak.kukaComm.Exceptions.RobotNotConfiguredException;
import com.wawrzyniak.kukaComm.Exceptions.WrongIpException;
import com.wawrzyniak.kukaComm.Model.ModelReading.ConfiguredRobot;
import com.wawrzyniak.kukaComm.Model.ModelReading.ConfiguredRobotDTO;
import com.wawrzyniak.kukaComm.Repository.ConfiguredRobotsRepository;
import lombok.RequiredArgsConstructor;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.List;
import java.util.Optional;
import java.util.regex.Pattern;

@Service
@RequiredArgsConstructor
public class ConfiguredRobotService {

    private final RobotMapper mapper;
    private final ConfiguredRobotsRepository robotRepository;

    public ConfiguredRobotDTO save(ConfiguredRobotDTO robotDTO) throws WrongIpException {
        ConfiguredRobot robotToSave = mapper.dtoToRobot(robotDTO);
        if (!validate(robotToSave.getIpAddress())) {
            throw new WrongIpException("Wrong IP address format");
        }

        ConfiguredRobot savedRobot = robotRepository.save(robotToSave);
        return this.mapper.robotToDto(savedRobot);
    }

    public ConfiguredRobotDTO updateByIp(ConfiguredRobotDTO robotDTO) throws RobotNotConfiguredException {
        Optional<ConfiguredRobot> robot = robotRepository.findByIpAddress(robotDTO.getIpAddress());
        if(robot.isEmpty()) {
            throw new RobotNotConfiguredException();
        }
        robotDTO.setId(robot.get().getId());
        ConfiguredRobot robotToUpdate = this.mapper.dtoToRobot(robotDTO);
        return this.mapper.robotToDto(this.robotRepository.save(robotToUpdate));
    }

    public void deleteByIp(String ipAddress) {
        Optional<ConfiguredRobot> robotToDelete = robotRepository.findByIpAddress(ipAddress);
        if (robotToDelete.isEmpty()) {
            return;
        }
        robotRepository.delete(robotToDelete.get());
    }

    public ConfiguredRobotDTO getRobotByIp(String ipAddress) throws RobotNotConfiguredException {
        Optional<ConfiguredRobot> robot = robotRepository.findByIpAddress(ipAddress);
        if(robot.isEmpty()) {
            throw new RobotNotConfiguredException();
        }
        return mapper.robotToDto(robot.get());
    }

    public List<ConfiguredRobotDTO> getAllConfiguredRobots() {
        List<ConfiguredRobot> robots = new ArrayList<>();
        Iterable<ConfiguredRobot> iterator = robotRepository.findAll();
        iterator.forEach(robots::add);
        return mapper.robotListToDto(robots);
    }

    private boolean validate(String ipAddress) {
        return Pattern.compile("^((25[0-5]|(2[0-4]|1\\d|[1-9]|)\\d)\\.?\\b){4}$").matcher(ipAddress).find();
    }
}
