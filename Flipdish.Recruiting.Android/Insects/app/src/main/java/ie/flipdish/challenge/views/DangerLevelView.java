package ie.flipdish.challenge.views;

import android.content.Context;
import android.graphics.Color;
import android.graphics.drawable.Drawable;
import android.graphics.drawable.GradientDrawable;
import android.os.Build;
import android.util.AttributeSet;

import androidx.annotation.RequiresApi;
import androidx.appcompat.widget.AppCompatTextView;

import ie.flipdish.challenge.R;
import ie.flipdish.challenge.adapter.InsectRecyclerAdapter;


public class DangerLevelView extends AppCompatTextView {

    private int mDangerLevel;

    public DangerLevelView(Context context) {
        super(context);
    }

    public DangerLevelView(Context context, AttributeSet attrs) {
        super(context, attrs);
    }

    public DangerLevelView(Context context, AttributeSet attrs, int defStyleAttr) {
        super(context, attrs, defStyleAttr);
    }

    public void setDangerLevel(int dangerLevel) {
        mDangerLevel = dangerLevel;
        this.setText(String.valueOf(dangerLevel));
    }

    public int getDangerLevel() {
        return mDangerLevel;
    }

    public void setDangerLevelColor(InsectRecyclerAdapter.InsectHolder holder){
        String[] colors = getContext().getResources().getStringArray(R.array.dangerColors);
        Drawable bg = null;
        if (android.os.Build.VERSION.SDK_INT >= android.os.Build.VERSION_CODES.LOLLIPOP) {
            bg = getContext().getResources().getDrawable(R.drawable.background_danger, null);
        }

        switch (holder.mDangerLevel.getDangerLevel()){
            case 1:
                ((GradientDrawable)bg).setColor(Color.parseColor(colors[0]));
                holder.mDangerLevel.setBackground(bg);
                break;
            case 2:
                ((GradientDrawable)bg).setColor(Color.parseColor(colors[1]));
                holder.mDangerLevel.setBackground(bg);
                break;
            case 3:
                ((GradientDrawable)bg).setColor(Color.parseColor(colors[2]));
                holder.mDangerLevel.setBackground(bg);
                break;
            case 4:
                ((GradientDrawable)bg).setColor(Color.parseColor(colors[3]));
                holder.mDangerLevel.setBackground(bg);
                break;
            case 5:
                ((GradientDrawable)bg).setColor(Color.parseColor(colors[4]));
                holder.mDangerLevel.setBackground(bg);
                break;
            case 6:
                ((GradientDrawable)bg).setColor(Color.parseColor(colors[5]));
                holder.mDangerLevel.setBackground(bg);
                break;
            case 7:
                ((GradientDrawable)bg).setColor(Color.parseColor(colors[6]));
                holder.mDangerLevel.setBackground(bg);
                break;
            case 8:
                ((GradientDrawable)bg).setColor(Color.parseColor(colors[7]));
                holder.mDangerLevel.setBackground(bg);
                break;
            case 9:
                ((GradientDrawable)bg).setColor(Color.parseColor(colors[8]));
                holder.mDangerLevel.setBackground(bg);
                break;
            case 10:
                ((GradientDrawable)bg).setColor(Color.parseColor(colors[9]));
                holder.mDangerLevel.setBackground(bg);
                break;
        }
    }
}
