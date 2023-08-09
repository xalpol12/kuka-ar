package com.wawrzyniak.kukaComm.Repository;

import org.springframework.beans.factory.annotation.Value;
import org.springframework.stereotype.Repository;

import java.io.File;
import java.io.FilenameFilter;
import java.util.ArrayList;
import java.util.List;

@Repository
public class RobotStickerRepository {

    @Value("${img.location}")
    private String location;
    private final FilenameFilter fileFilter;

<<<<<<< refs/remotes/origin/main
<<<<<<< refs/remotes/origin/main
    RobotStickerRepository() {
=======
    RobotStickerRepository(){
>>>>>>> add testSocket and kukaComm
=======
    RobotStickerRepository() {
>>>>>>> add swagger docs, fix some whitespace issues
        fileFilter = new ImageFileFilter(".png", ".jpg");
    }

    public List<File> getStickers() {
        File dir = new File(location);
        File[] files = dir.listFiles(fileFilter);
        if (files == null) {
            return new ArrayList<>();
        }
        return new ArrayList<>(List.of(files));
    }

    private class ImageFileFilter implements FilenameFilter {

        private final String[] exts;

<<<<<<< refs/remotes/origin/main
<<<<<<< refs/remotes/origin/main
        ImageFileFilter(String... extensions) {
=======
        ImageFileFilter(String... extensions){
>>>>>>> add testSocket and kukaComm
=======
        ImageFileFilter(String... extensions) {
>>>>>>> add swagger docs, fix some whitespace issues
            this.exts = extensions;
        }

        @Override
        public boolean accept(File dir, String name) {
            for(String ext : exts){
                if (name.endsWith(ext)){
                    return true;
                }
            }
            return false;
        }
    }
}
