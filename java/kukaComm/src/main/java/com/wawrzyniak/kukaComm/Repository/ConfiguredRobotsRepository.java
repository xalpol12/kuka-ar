package com.wawrzyniak.kukaComm.Repository;

import com.wawrzyniak.kukaComm.Model.ModelReading.ConfiguredRobot;
import org.springframework.data.repository.CrudRepository;
import org.springframework.stereotype.Repository;

import java.util.Optional;

@Repository
public interface ConfiguredRobotsRepository extends CrudRepository<ConfiguredRobot, Long> {
    public Optional<ConfiguredRobot> findByIpAddress(String ipAddress);
}
