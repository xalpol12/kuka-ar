package com.wawrzyniak.kukaComm.Service.RobotData;

import com.wawrzyniak.kukaComm.Exceptions.RobotAlredyConfiguredException;
import com.wawrzyniak.kukaComm.Exceptions.RobotNotConfiguredException;
import com.wawrzyniak.kukaComm.Exceptions.WrongIpException;
import com.wawrzyniak.kukaComm.Model.ModelReading.ConfiguredRobotDTO;
import com.wawrzyniak.kukaComm.Repository.ConfiguredRobotsRepository;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.context.SpringBootTest;

import java.util.List;

import static org.junit.jupiter.api.Assertions.*;

@SpringBootTest
class ConfiguredRobotServiceTest {

    @Autowired
    private ConfiguredRobotService robotService;
    @Autowired
    private ConfiguredRobotsRepository robotsRepository;

    @BeforeEach
    public void clearDatabase(){
        robotsRepository.deleteAll();
    }

    @Test
    void save() throws WrongIpException, RobotAlredyConfiguredException {
        //given
        ConfiguredRobotDTO robotDTO = new ConfiguredRobotDTO();
        robotDTO.setId(1L);
        robotDTO.setName("kukaTest");
        robotDTO.setCategory("Tests");
        robotDTO.setIpAddress("192.168.1.50");
        //when
        ConfiguredRobotDTO savedRobot = robotService.save(robotDTO);
        //then
        assertNotNull(savedRobot.getId());
        assertEquals(robotDTO.getCategory(), savedRobot.getCategory());
        assertEquals(robotDTO.getIpAddress(), savedRobot.getIpAddress());
        assertEquals(robotDTO.getName(), savedRobot.getName());
    }

    @Test
    void saveWithInvalidIp() {
        // given
        ConfiguredRobotDTO robotDTO = new ConfiguredRobotDTO();
        robotDTO.setId(1L);
        robotDTO.setName("kukaTest");
        robotDTO.setCategory("Tests");
        robotDTO.setIpAddress("1921.168.1.520");

        try {
            // when
            robotService.save(robotDTO);
        } catch (WrongIpException e) {
            // then
            assertEquals("Wrong IP address format", e.getMessage());
        } catch (RobotAlredyConfiguredException e) {
            throw new RuntimeException(e);
        }
    }

    @Test
    void update() throws RobotNotConfiguredException, WrongIpException, RobotAlredyConfiguredException {
        //given
        ConfiguredRobotDTO robotDTO = new ConfiguredRobotDTO();
        robotDTO.setId(1L);
        robotDTO.setName("kukaTest");
        robotDTO.setCategory("Tests");
        robotDTO.setIpAddress("192.168.1.50");

        ConfiguredRobotDTO savedRobot = robotService.save(robotDTO);

        ConfiguredRobotDTO newRobot = new ConfiguredRobotDTO();
        newRobot.setId(savedRobot.getId());
        newRobot.setCategory("EditTests");
        newRobot.setName("editedKuka");
        newRobot.setIpAddress("192.168.1.50");
        //when
        ConfiguredRobotDTO edited = robotService.updateByIp(newRobot);
        //then
        assertEquals(savedRobot.getId(), edited.getId());
        assertEquals(newRobot.getName(), edited.getName());
        assertEquals(newRobot.getCategory(), edited.getCategory());
        assertEquals(newRobot.getIpAddress(), edited.getIpAddress());

    }

    @Test
    void deleteByIp() throws WrongIpException, RobotAlredyConfiguredException {
        //given
        ConfiguredRobotDTO robotDTO = new ConfiguredRobotDTO();
        robotDTO.setId(1L);
        robotDTO.setName("kukaTest");
        robotDTO.setCategory("Tests");
        robotDTO.setIpAddress("192.168.1.50");

        ConfiguredRobotDTO savedRobot = robotService.save(robotDTO);
        long savedRobotLength = robotsRepository.count();
        //when
        robotService.deleteByIp(savedRobot.getIpAddress());
        long savedRobotLengthAfter = robotsRepository.count();
        //then
        assertEquals(1, savedRobotLength);
        assertEquals(0, savedRobotLengthAfter);
    }

    @Test
    void getRobotByIp() throws RobotNotConfiguredException, WrongIpException, RobotAlredyConfiguredException {
        //given
        ConfiguredRobotDTO robotDTO = new ConfiguredRobotDTO();
        robotDTO.setId(1L);
        robotDTO.setName("kukaTest");
        robotDTO.setCategory("Tests");
        robotDTO.setIpAddress("192.168.1.50");

        robotService.save(robotDTO);
        //when
        ConfiguredRobotDTO foundRobot = robotService.getRobotByIp(robotDTO.getIpAddress());
        //then
        assertNotNull(foundRobot.getId());
        assertEquals(robotDTO.getIpAddress(), foundRobot.getIpAddress());
        assertEquals(robotDTO.getName(), foundRobot.getName());
        assertEquals(robotDTO.getCategory(), foundRobot.getCategory());
    }

    @Test
    void getAllConfiguredRobots() throws WrongIpException, RobotAlredyConfiguredException {
        //given
        ConfiguredRobotDTO robotDTO_0 = new ConfiguredRobotDTO();
        robotDTO_0.setId(1L);
        robotDTO_0.setName("kukaTest");
        robotDTO_0.setCategory("Tests");
        robotDTO_0.setIpAddress("192.168.1.55");
        ConfiguredRobotDTO robotDTO_1 = new ConfiguredRobotDTO();
        robotDTO_1.setId(2L);
        robotDTO_1.setName("kukaTest");
        robotDTO_1.setCategory("Tests");
        robotDTO_1.setIpAddress("192.168.1.51");
        ConfiguredRobotDTO robotDTO_2 = new ConfiguredRobotDTO();
        robotDTO_2.setId(3L);
        robotDTO_2.setName("kukaTest");
        robotDTO_2.setCategory("Tests");
        robotDTO_2.setIpAddress("192.168.1.52");
        robotService.save(robotDTO_0);
        robotService.save(robotDTO_1);
        robotService.save(robotDTO_2);
        //when
        List<ConfiguredRobotDTO> robots = robotService.getAllConfiguredRobots();
        //then
        assertEquals(robotsRepository.count(), robots.size());
    }

    @Test
    void shouldThrowRobotAlreadyConfiguredException()
            throws RobotAlredyConfiguredException, WrongIpException {
        ConfiguredRobotDTO originalRobotDTO = getRobotDTO(1L, "192.168.1.50");
        ConfiguredRobotDTO duplicateIpRobotDTO = getRobotDTO(2L, "192.168.1.50");

        robotService.save(originalRobotDTO);

        assertThrows(RobotAlredyConfiguredException.class,
                () -> robotService.save(originalRobotDTO));
        assertThrows(RobotAlredyConfiguredException.class,
                () -> robotService.save(duplicateIpRobotDTO));
    }

    private ConfiguredRobotDTO getRobotDTO(long id, String ipAddress) {
        ConfiguredRobotDTO dto = new ConfiguredRobotDTO();
        dto.setId(id);
        dto.setIpAddress(ipAddress);
        dto.setName("kukaTest");
        dto.setCategory("Tests");
        return dto;
    }
}