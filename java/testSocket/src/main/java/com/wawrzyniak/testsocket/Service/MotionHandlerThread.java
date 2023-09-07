package com.wawrzyniak.testsocket.Service;

import com.wawrzyniak.testsocket.Model.KRLVar;
import com.wawrzyniak.testsocket.Model.MotionDescription;
import com.wawrzyniak.testsocket.Model.Records.Vector3;
import com.wawrzyniak.testsocket.Model.Value.KRLFrame;

import java.util.Queue;
import java.util.concurrent.ConcurrentLinkedQueue;

public class MotionHandlerThread extends Thread{

    private final Queue<MotionDescription> motions;
    private final KRLVar tcpPosition;

    public MotionHandlerThread(KRLVar tcp){
        tcpPosition = tcp;
        motions = new ConcurrentLinkedQueue<>();
    }
    public void addMotion(MotionDescription md) {
        motions.add(md);
    }

    @Override
    public void run() {
        while(!Thread.currentThread().isInterrupted()) {
            while (!motions.isEmpty()) {
                MotionDescription motion = motions.poll();
                int steps = motion.getDesiredTime()/5;
                KRLFrame tcp = (KRLFrame) tcpPosition.getValue();
                Vector3 distance = tcp.calculatePositionShift(motion.getTcpPosition().getPosition());
                Vector3 rotation = tcp.calculateRotationShift(motion.getTcpPosition().getRotation());
                Vector3 distanceStep = distance.divide(steps);
                Vector3 rotationStep = rotation.divide(steps);
                for (int i = 0; i < steps; i++){
                    tcp.transform(distanceStep);
                    tcp.rotate(rotationStep);
                    tcpPosition.setValue(tcp);
                    try {
                        Thread.sleep(5);
                    } catch (InterruptedException e){
                        Thread.currentThread().interrupt();
                    }
                }
                tcpPosition.setValue(motion.getTcpPosition());
            }

            try {
                Thread.sleep(5);
            } catch (InterruptedException e){
                Thread.currentThread().interrupt();
            }
        }
    }
}
