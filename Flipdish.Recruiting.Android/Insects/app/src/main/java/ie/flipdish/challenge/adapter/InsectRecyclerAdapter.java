package ie.flipdish.challenge.adapter;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;


import androidx.recyclerview.widget.RecyclerView;

import java.util.ArrayList;
import java.util.Collections;
import java.util.List;

import ie.flipdish.challenge.R;
import ie.flipdish.challenge.models.Insect;
import ie.flipdish.challenge.views.DangerLevelView;


/**
 * RecyclerView adapter extended with project-specific required methods.
 */

public class InsectRecyclerAdapter extends
        RecyclerView.Adapter<InsectRecyclerAdapter.InsectHolder> {

    /* ViewHolder for each insect item */
    public class InsectHolder extends RecyclerView.ViewHolder implements View.OnClickListener {

        public final DangerLevelView mDangerLevel;
        public final TextView mCommonName;
        public final TextView mScientificName;

        public InsectHolder(View itemView) {
            super(itemView);
            mDangerLevel = (DangerLevelView) itemView.findViewById(R.id.dangerLevel);
            mCommonName = (TextView) itemView.findViewById(R.id.commonName);
            mScientificName = (TextView) itemView.findViewById(R.id.scientificName);
            itemView.setOnClickListener(this);
        }

        @Override
        public void onClick(View v) {
            mOnItemClickListener.onItemClick(getItem(getAdapterPosition()));
        }
    }

    private  LayoutInflater mInflater;
    private  RecyclerOnItemClickListener mOnItemClickListener;
    public List<Insect> mInsectsList = new ArrayList<>();

    public InsectRecyclerAdapter(Context context, RecyclerOnItemClickListener mOnItemClickListener) {
        mInflater = LayoutInflater.from(context);
        this.mOnItemClickListener = mOnItemClickListener;
    }

    public void setData(List<Insect> insectsList) {
        mInsectsList = insectsList;
        notifyDataSetChanged();
    }

    @Override
    public InsectHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        View view = mInflater.inflate(R.layout.insect_list_item, parent, false);
        return new InsectHolder(view);
    }


    @Override
    public void onBindViewHolder(InsectHolder holder, int position) {
        holder.mCommonName.setText(mInsectsList.get(position).name);
        holder.mScientificName.setText(mInsectsList.get(position).scientificName);
        holder.mDangerLevel.setDangerLevel(mInsectsList.get(position).dangerLevel);
        holder.mDangerLevel.setDangerLevelColor(holder);
    }

    @Override
    public int getItemCount() {
       return (getInsectsList().size() > 0 ) ? getInsectsList().size() : -1;
    }

    /**
     * Return the {@link Insect} represented by this item in the adapter.
     *
     * @param position Adapter item position.
     *
     * @return A new {@link Insect} filled with this position's attributes
     *
     * @throws IllegalArgumentException if position is out of the adapter's bounds.
     */
    public Insect getItem(int position) {
        if (position < 0 || position >= getItemCount())
            throw new IllegalArgumentException("Item position is out of adapter's range");
        return getInsectsList().get(position);
    }


    public List<Insect> getInsectsList(){
        Collections.shuffle(mInsectsList);
        return mInsectsList;
    }

    public interface RecyclerOnItemClickListener {
        /**
         * Called when an item is clicked.
         * @param insect  Position of the insect that was clicked.
         */
        void onItemClick(Insect insect);
    }
}
