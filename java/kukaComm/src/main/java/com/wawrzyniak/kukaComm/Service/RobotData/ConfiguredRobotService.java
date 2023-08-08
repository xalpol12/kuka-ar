package com.wawrzyniak.kukaComm.Service.RobotData;

import com.wawrzyniak.kukaComm.Exceptions.RobotNotConfiguredException;
import com.wawrzyniak.kukaComm.Model.ModelReading.ConfiguredRobot;
import com.wawrzyniak.kukaComm.Model.ModelReading.ConfiguredRobotDTO;
import com.wawrzyniak.kukaComm.Repository.ConfiguredRobotsRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.List;
import java.util.Optional;

@Service
public class ConfiguredRobotService {

    private final RobotMapper mapper;
    private final ConfiguredRobotsRepository robotRepository;
    @Autowired
    public ConfiguredRobotService(RobotMapper mapper, ConfiguredRobotsRepository robotRepository) {
        this.mapper = mapper;
        this.robotRepository = robotRepository;
    }

    public ConfiguredRobotDTO save(ConfiguredRobotDTO robotDTO) {
        ConfiguredRobot robotToSave = mapper.dtoToRobot(robotDTO);
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

    public void deleteByIp(String ipAddress){
        Optional<ConfiguredRobot> robotToDelete = robotRepository.findByIpAddress(ipAddress);
        if(robotToDelete.isEmpty()){
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

    public List<ConfiguredRobotDTO> getAllConfiguredRobots(){
        List<ConfiguredRobot> robots = new ArrayList<>();
        Iterable<ConfiguredRobot> iterator = robotRepository.findAll();
        iterator.forEach(robots::add);
        return mapper.robotListToDto(robots);
    }

}
