//
//  DogListViewController.swift
//  DogSpot
//
//  Created by Stephen Walsh on 13/01/2021.
//

import UIKit

/// This codebase has gone to the dogs 😼

final class DogListViewController: UIViewController,
                                   UICollectionViewDataSource,
                                   UICollectionViewDelegateFlowLayout,
                                   UICollectionViewDelegate,
                                   Delegate, DetailsDelegate {
    
    static let apiBase = "https://api.thedogapi.com/v1/"
    var apikey = "081c16a8-75b7-44ae-b6db-9fd93bf68903";
    var session: URLSession!
    
    var items: [[Dog]] = [];
    var favouriteIds: [Int] = [];
    
    var collectionView: UICollectionView = {
        var layout = UICollectionViewFlowLayout();
        layout.scrollDirection = .vertical;
        
        var collectionView = UICollectionView(frame: .zero,
                                              collectionViewLayout: layout);
        
        collectionView.register(ListItemCell.self,
                                forCellWithReuseIdentifier: "ListItemCell")
        
        collectionView.register(CardCollectionCell.self,
                                forCellWithReuseIdentifier: "CardCollectionCell");
        
        collectionView.register(SectionTitleView.self,
                                forSupplementaryViewOfKind: UICollectionView.elementKindSectionHeader,
                                withReuseIdentifier: "SectionTitleView");
        
        collectionView.allowsSelection = true
        collectionView.alwaysBounceVertical = true
        
        return collectionView
    }();
    
    override func viewDidLoad() {
        setupSubviews();
        applyConstraints();
        
        Timer.scheduledTimer(withTimeInterval: 0.25, repeats: true) { timer in
            self.refresh();
        }
    }
    
    override func viewDidAppear(_ animated: Bool) {
        getDogs();
    }
    
    func setupSubviews() {
        view.backgroundColor = Palette().backgroundGray;
        
        collectionView.backgroundColor = Palette().backgroundGray;
        
        setupTitleView();
        
        view.addSubview(collectionView)
        collectionView.translatesAutoresizingMaskIntoConstraints = false;
        
        collectionView.dataSource = self;
        collectionView.delegate = self;
        collectionView.reloadData();
    }
    
    func setupTitleView() {
        title = "Dogs Of The World";
        
        navigationController?.navigationBar.largeTitleTextAttributes = [
            NSAttributedString.Key.font: TypeMachine().titleFont,
        ]
        
        navigationController?.navigationBar.prefersLargeTitles = true
        navigationController?.navigationItem.largeTitleDisplayMode = .always
        navigationController?.navigationBar.tintColor = Palette().actionRed
    }
    
    func applyConstraints() {
        
        NSLayoutConstraint.activate([
            
            /// collectionView constraints
            
            collectionView.topAnchor.constraint(equalTo: view.safeAreaLayoutGuide.topAnchor),
            collectionView.leadingAnchor.constraint(equalTo: view.safeAreaLayoutGuide.leadingAnchor),
            collectionView.trailingAnchor.constraint(equalTo: view.safeAreaLayoutGuide.trailingAnchor),
            collectionView.bottomAnchor.constraint(equalTo: view.safeAreaLayoutGuide.bottomAnchor),
            
        ])
    }
    
    func numberOfSections(in collectionView: UICollectionView) -> Int {
        return items.count;
    }
    
    func collectionView(_ collectionView: UICollectionView,
                        numberOfItemsInSection section: Int) -> Int {
        return items[section].count;
    }
    
    func collectionView(_ collectionView: UICollectionView,
                        cellForItemAt indexPath: IndexPath) -> UICollectionViewCell {
        var item = items[indexPath.section][indexPath.item]
        
        if indexPath.section == 0 {
            
            var cell = collectionView.dequeueReusableCell(withReuseIdentifier: "CardCollectionCell",
                                                          for: indexPath) as! CardCollectionCell
            
            cell.items = items[indexPath.section];
            cell.delegate = self;
            
            return cell;
            
        }
        
        var cell = collectionView.dequeueReusableCell(withReuseIdentifier: "ListItemCell",
                                                      for: indexPath) as! ListItemCell
        
        cell.titleLabel.text = item.name;
        
        return cell;
    }
    
    func tapCardCell(indexPath: IndexPath) {
        let item = items[indexPath.section][indexPath.row];
        
        let view = DogDetailsViewController();
        view.dog = item;
        view.isFavorite = favouriteIds.contains(item.id);
        
        navigationController?.pushViewController(view, animated: true);
    }
    
    func collectionView(_ collectionView: UICollectionView,
                        didSelectItemAt indexPath: IndexPath) {
        let item = items[indexPath.section][indexPath.item];
        
        let view = DogDetailsViewController();
        view.dog = item;
        view.delegate = self;
        view.isFavorite = favouriteIds.contains(item.id);
        
        navigationController?.pushViewController(view, animated: true);
    }
    
    func collectionView(_ collectionView: UICollectionView,
                        layout collectionViewLayout: UICollectionViewLayout,
                        sizeForItemAt indexPath: IndexPath) -> CGSize {
        if indexPath.section == 0 {
            if favouriteIds.count > 0 {
                return CGSize(width: collectionView.frame.size.width, height: 175.0);
            }
        }
        return CGSize(width: collectionView.frame.size.width, height: 67.0);
    }
    
    func collectionView(_ collectionView: UICollectionView,
                        viewForSupplementaryElementOfKind kind: String,
                        at indexPath: IndexPath) -> UICollectionReusableView {
        
        let headerView = collectionView.dequeueReusableSupplementaryView(ofKind: kind,
                                                                         withReuseIdentifier: "SectionTitleView",
                                                                         for: indexPath) as! SectionTitleView;
        
        if indexPath.section == 0 {
            headerView.titleLabel.text = "Favourites";
            headerView.dotStrokeColor = Palette().actionRed.cgColor;
        } else {
            headerView.titleLabel.text = "Everypawdy";
            headerView.dotStrokeColor = Palette().floatyBlue.cgColor;
        }
        headerView.frame.size.height = 100;
        
        return headerView;
    }
    
    func collectionView(_ collectionView: UICollectionView,
                        layout collectionViewLayout: UICollectionViewLayout,
                        referenceSizeForHeaderInSection section: Int) -> CGSize {
        return CGSize(width: collectionView.frame.width, height: 100);
    }
    
    func getDogs() {
        self.session = URLSession.shared;
                                
                                var urlString = DogListViewController.apiBase+"/breeds?page=0&limit=10000";
                                var url = URL(string: urlString)!;
                                var request = URLRequest(url: url);
                                
                                var allDogs: [Dog] = [];
                                
                                URLSession.shared.dataTask(with: request) { (data, response, error) in
                                    
                                    if error != nil {
                                        let alertView = UIAlertController(title: "Error!",
                                                                          message: error!.localizedDescription,
                                                  preferredStyle: .alert)
                self.present(alertView, animated: true, completion: nil);
                return;
            }
            
            do {
                let rootJSON:NSArray = try JSONSerialization.jsonObject(with: data!, options: .mutableContainers) as! NSArray;
                
                var i = 0;
                while i < rootJSON.count {
                                            var dogJSON = rootJSON[i] as! NSDictionary;
                                            var dog = Dog()
                                            dog.name = dogJSON["name"] as! String;
                                            dog.id = dogJSON["id"] as! Int;

                                            var image = Image()
                                            image.id = (dogJSON["image"] as! NSDictionary)["id"] as! String;
                                            dog.image = image;

                                            var weight = Weight();

                                            weight.metric = (dogJSON["weight"] as! NSDictionary)["metric"] as! String;
                                            dog.weight = weight;
                                            dog.life_span = dogJSON["life_span"] as! String;

                                            allDogs.append(dog);
                                            i+=1;
                                            }
                
            } catch {
                print(error);
                
                let alertView = UIAlertController(title: "Error!",
                                                  message: error.localizedDescription,
                                                  preferredStyle: .alert);
                self.present(alertView, animated: true, completion: nil);
            }
            
            var favorites: [Dog] = [];
            
            for dog in allDogs {
                if self.favouriteIds.contains(dog.id) {
                    favorites.append(dog);
                }
            }
            
            self.items = [favorites, allDogs];
        }.resume();
    }
    
    /***
     This timer carry-on may look unrealistically bad, but I've seen worse...
     */
    
    func refresh() {
        self.collectionView.reloadData();
    }
    
    func didFavoriteDog(id: Int) {
    favouriteIds.append(id);
    collectionView.reloadData();
    }
}
